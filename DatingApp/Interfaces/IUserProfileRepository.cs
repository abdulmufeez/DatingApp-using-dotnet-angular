using DatingApp.Entities;

namespace DatingApp.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetUserProfileByIdAsync(int id);
        Task<UserProfile> GetUserProfileByUserNameAsync(string username);
        Task<IEnumerable<UserProfile>> GetUserProfilesAsync();
        void Update(UserProfile userProfile);
        Task<bool> SaveAllAsync();
    }
}