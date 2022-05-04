using DatingApp.DTOs;
using DatingApp.Entities;

namespace DatingApp.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int LikeUserId);
        Task<UserProfile> GetUserProfileWithLikes(int userProfileId);
        Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userProfileId);
    }
}