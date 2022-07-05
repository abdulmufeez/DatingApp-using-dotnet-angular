using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Helpers;

namespace DatingApp.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetUserByIdAsync(int id);   
        Task<UserProfile> GetUserByAppIdAsync(int appId);
        Task<UserProfile> GetUserByUserNameAsync(string username);             
        Task<UserProfileDto> GetUserProfileByAppIdAsync(int id, bool isCurrentUser);        
        Task<UserProfileDto> GetUserProfileByIdAsync(int id);            
        Task<PagedList<UserProfileDto>> GetUserProfilesAsync(UserProfileParams userProfileParams);
        Task<UserProfile> GetUserProfileByPhotoId(int photoId);
        Task<string> GetUserGender(int id);
        void Add(UserProfile userProfile);
        void Update(UserProfile userProfile);        
    }
}