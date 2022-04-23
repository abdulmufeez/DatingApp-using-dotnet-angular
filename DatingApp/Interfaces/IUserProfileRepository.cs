using DatingApp.DTOs;
using DatingApp.Entities;

namespace DatingApp.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetUserByIdAsync(int id);   
        Task<UserProfile> GetUserByUserNameAsync(string username);             
        Task<UserProfileDto> GetUserProfileByAppIdAsync(int id);        
        Task<UserProfileDto> GetUserProfileByIdAsync(int id);
        Task<IEnumerable<UserProfileDto>> GetUserProfilesAsync();
        void Update(UserProfile userProfile);
        Task<bool> SaveAllAsync();
    }
}