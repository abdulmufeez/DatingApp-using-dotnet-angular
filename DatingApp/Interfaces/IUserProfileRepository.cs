using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Helpers;

namespace DatingApp.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetUserByIdAsync(int id);   
        Task<UserProfile> GetUserByAppIdAsync(int appId);                         
        Task<UserProfile> GetUserByPhotoId(int photoId);
        Task<UserProfileDto> GetUserProfileByUsernameAsync(string username, bool isCurrentUser);            
        Task<PagedList<UserProfileDto>> GetUserProfilesAsync(UserProfileParams userProfileParams);        
        Task<string> GetUserGender(int id);
        void Add(UserProfile userProfile);
        void Update(UserProfile userProfile);        
    }
}