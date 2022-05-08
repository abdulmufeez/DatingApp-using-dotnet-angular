using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Helpers;

namespace DatingApp.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int LikeUserId);
        Task<UserProfile> GetUserProfileWithLikes(int userProfileId);
        Task<PagedList<LikeDto>> GetUserLikes(LikeParams likeParams);
    }
}