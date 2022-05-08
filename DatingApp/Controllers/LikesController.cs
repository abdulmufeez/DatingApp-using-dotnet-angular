using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ILikesRepository _likeRepository;

        public LikesController(IUserProfileRepository userProfileRepository, ILikesRepository likeRepository)
        {
            _userProfileRepository = userProfileRepository;
            _likeRepository = likeRepository;
        }

        [HttpPost("{likedUserId}")]
        public async Task<ActionResult> AddLike(int likedUserId)
        {
            var sourceUserProfileId = (await _userProfileRepository.GetUserByAppIdAsync(User.GetAppUserId())).Id;
            var likedUserProfile = await _userProfileRepository.GetUserByIdAsync(likedUserId);
            var sourceUserProfile = await _likeRepository.GetUserProfileWithLikes(sourceUserProfileId);

            if (likedUserProfile is null) return NotFound();
            // not like by itself
            if (sourceUserProfile.Id == likedUserId) return BadRequest("You cannot like yourself!");

            var userLike = await _likeRepository.GetUserLike(sourceUserProfileId, likedUserProfile.Id);
            // check if already liked
            if (userLike is not null) return BadRequest($"You already liked {likedUserProfile.KnownAs}");

            userLike = new UserLike
            {
                SourceUserId = sourceUserProfileId,
                LikedUserId = likedUserProfile.Id
            };
            sourceUserProfile.LikedUsers.Add(userLike);
            if (await _userProfileRepository.SaveAllAsync()) return Ok();

            return BadRequest($"Failed to like {likedUserProfile.KnownAs}");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikeParams likeParams)        
        {
            likeParams.UserProfileId = (await _userProfileRepository.GetUserByAppIdAsync(User.GetAppUserId())).Id;
            var userProfiles = await _likeRepository.GetUserLikes(likeParams);
            Response.AddPaginationHeader(userProfiles.CurrentPage,userProfiles.PageSize,userProfiles.TotalCount,userProfiles.TotalPages);
            return Ok(userProfiles);
        }
    }
}