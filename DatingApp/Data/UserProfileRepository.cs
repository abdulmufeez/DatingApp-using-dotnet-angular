using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly DataContext _context;

        public UserProfileRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserProfile> GetUserProfileByIdAsync(int id)
        {
            return await _context.UserProfile.FindAsync(id);
        }

        public async Task<UserProfile> GetUserProfileByUserNameAsync(string knownas)
        {
            return await _context.UserProfile
                .Include(p => p.Photos)                
                .SingleOrDefaultAsync(x => x.KnownAs == knownas);
        }

        public async Task<IEnumerable<UserProfile>> GetUserProfilesAsync()
        {
            return await _context.UserProfile
                .Include(p => p.Photos)
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
    }
}