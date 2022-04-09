using DatingApp.DTOs;
using DatingApp.Entities;

namespace DatingApp.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetUserProfileByIdAsync(int id);
        Task<UserProfileDto> GetUserProfileByUserNameAsync(string username);
        Task<IEnumerable<UserProfileDto>> GetUserProfilesAsync();
        void Update(UserProfile userProfile);
        Task<bool> SaveAllAsync();
    }
}