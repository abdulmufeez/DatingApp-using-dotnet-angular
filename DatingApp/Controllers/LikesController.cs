using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Extensions;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Authorize]
    public class LikesController : BaseController
    {       
        private readonly IUnitOfWork _unitOfWork;
        public LikesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }

        [HttpPost("{likedUserId}")]
        public async Task<ActionResult> AddLike(int likedUserId)
        {
            var sourceUserProfileId = (await _unitOfWork.UserProfileRepository.GetUserByAppIdAsync(User.GetAppUserId())).Id;
            var likedUserProfile = await _unitOfWork.UserProfileRepository.GetUserByIdAsync(likedUserId);
            var sourceUserProfile = await _unitOfWork.LikeRepository.GetUserProfileWithLikes(sourceUserProfileId);

            if (likedUserProfile is null) return NotFound();
            // not like by itself
            if (sourceUserProfile.Id == likedUserId) return BadRequest("You cannot like yourself!");

            var userLike = await _unitOfWork.LikeRepository.GetUserLike(sourceUserProfileId, likedUserProfile.Id);
            // check if already liked
            if (userLike is not null) return BadRequest($"You already liked {likedUserProfile.KnownAs}");

            userLike = new UserLike
            {
                SourceUserId = sourceUserProfileId,
                LikedUserId = likedUserProfile.Id
            };
            sourceUserProfile.LikedUsers.Add(userLike);
            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest($"Failed to like {likedUserProfile.KnownAs}");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikeParams likeParams)        
        {
            likeParams.UserProfileId = (await _unitOfWork.UserProfileRepository.GetUserByAppIdAsync(User.GetAppUserId())).Id;
            var userProfiles = await _unitOfWork.LikeRepository.GetUserLikes(likeParams);
            Response.AddPaginationHeader(userProfiles.CurrentPage,userProfiles.PageSize,userProfiles.TotalCount,userProfiles.TotalPages);
            return Ok(userProfiles);
        }
    }
}