using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserProfileRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserProfile> GetUserByIdAsync(int id)
        {
            return await _context.UserProfile.FindAsync(id);
        }

        public async Task<UserProfile> GetUserByAppIdAsync(int appId)
        {
            return await _context.UserProfile
                .Include(m => m.Photos)
                .SingleOrDefaultAsync(m => m.ApplicationUserId == appId);
        }

        public async Task<UserProfileDto> GetUserProfileByUsernameAsync(string username, bool isCurrentUser)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.UserName == username);

            var query = _context.UserProfile
                .Where(u => u.ApplicationUserId == user.Id)
                .ProjectTo<UserProfileDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            if (isCurrentUser) query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync();
        }

        // project to automatically map to dto leaving one further asign thing behind 
        // and there is also no need to use include or anything
        public async Task<PagedList<UserProfileDto>> GetUserProfilesAsync(UserProfileParams userProfileParams)
        {
            var query = _context.UserProfile
                .Include(u => u.ApplicationUser)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(userProfileParams.Search))
            {
                if (userProfileParams.DisableFiltering == "true")
                {
                    query = query.Where(query => query.ApplicationUser.UserName.Contains(userProfileParams.Search));
                }
                if (userProfileParams.DisableFiltering == "false")
                {
                    query = query.Where(query => query.ApplicationUser.UserName.Contains(userProfileParams.Search));
                    // Filtering result
                    query = query.Where(u => u.isDisabled == false);
                    query = query.Where(u => u.ApplicationUserId != userProfileParams.CurrentUserId);
                    query = query.Where(u => u.Gender == userProfileParams.Gender);

                    var minDob = DateTime.Today.AddYears(-userProfileParams.MaxAge - 1);
                    var maxDob = DateTime.Today.AddYears(-userProfileParams.MinAge);

                    query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

                    query = userProfileParams.OrderBy switch
                    {
                        "created" => query.OrderByDescending(u => u.ProfileCreatedAt),
                        _ => query.OrderByDescending(u => u.LastActive)
                    };
                }
            }
            else
            {
                query = query.Where(u => u.isDisabled == false);
                query = query.Where(u => u.ApplicationUserId != userProfileParams.CurrentUserId);
                query = query.Where(u => u.Gender == userProfileParams.Gender);

                var minDob = DateTime.Today.AddYears(-userProfileParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userProfileParams.MinAge);

                query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

                query = userProfileParams.OrderBy switch
                {
                    "created" => query.OrderByDescending(u => u.ProfileCreatedAt),
                    _ => query.OrderByDescending(u => u.LastActive)
                };
            }

            return await PagedList<UserProfileDto>.CreateAsync(
                query.ProjectTo<UserProfileDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(),
                    userProfileParams.PageNumber, userProfileParams.PageSize);
        }


        public void Add(UserProfile userProfile)
        {
            _context.Entry(userProfile).State = EntityState.Added;
            //_context.UserProfile.Add(userProfile);
        }

        //Marking an Entity that has modified or not
        public void Update(UserProfile userProfile)
        {
            _context.Entry(userProfile).State = EntityState.Modified;
        }

        public Task<string> GetUserGender(int id)
        {
            return _context.UserProfile.Where(u => u.ApplicationUserId == id)
                .Select(u => u.Gender)
                .FirstOrDefaultAsync();
        }

        public async Task<UserProfile> GetUserByPhotoId(int photoId)
        {
            return await _context.UserProfile
                .Include(p => p.Photos)
                .IgnoreQueryFilters()
                .Where(p => p.Photos.Any(p => p.Id == photoId))
                .FirstOrDefaultAsync();
        }

        // ===============================================================================
        // ===============================================================================        
        // public async Task<UserProfileDto> GetUserProfileByUserNameAsync(string knownas)
        // {
        //     return await _context.UserProfile
        //         .Where(x => x.KnownAs == knownas)
        //         .Select(user => new UserProfileDto{
        //             FirstName = user.FirstName,
        //             LastName = user.LastName,
        //             //and so on we can manually map directly Dto
        //             //without assign data to actual entity which save one round
        // but this create a lots work to do we can achieve this by using mapper
        //         })
        //         .SingleOrDefaultAsync();
        // }

        // public async Task<IEnumerable<UserProfile>> GetUserProfilesAsync()
        // {
        //     return await _context.UserProfile
        //         .Include(p => p.Photos)
        //         .ToListAsync();
        // }
    }
}