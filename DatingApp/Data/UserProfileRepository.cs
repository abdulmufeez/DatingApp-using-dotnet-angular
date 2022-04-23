using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.DTOs;
using DatingApp.Entities;
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

        public async Task<UserProfile> GetUserByUserNameAsync(string username)
        {
            var user = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.UserName == username);

            var userProfile = await _context.UserProfile
                .Include(m => m.Photos)
                .SingleOrDefaultAsync(m => m.ApplicationUserId == user.Id);

            return userProfile;
        }

        public async Task<UserProfileDto> GetUserProfileByAppIdAsync(int id)
        {
            return await _context.UserProfile                
                .ProjectTo<UserProfileDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(model => model.ApplicationUserId == id);
        }

        public async Task<UserProfileDto> GetUserProfileByIdAsync(int id)
        {
            return await _context.UserProfile
                .ProjectTo<UserProfileDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(model => model.Id == id);
        }

        // project to automatically map to dto leaving one further asign thing behind 
        // and there is also no need to use include or anything
        public async Task<IEnumerable<UserProfileDto>> GetUserProfilesAsync()
        {
            return await _context.UserProfile
                .ProjectTo<UserProfileDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        //Marking an Entity that has modified or not
        public void Update(UserProfile userProfile)
        {
            _context.Entry(userProfile).State = EntityState.Modified;
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