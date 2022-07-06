using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
    public class AdminController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public AdminController(UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IPhotoService photoService)
        {
            _photoService = photoService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName)
                .Include(u => u.UserProfile)
                .ThenInclude(u => u.Photos)
                .Select(u => new                // Select project anything to request response means sends 
                {                               // new {} => this will create an anonymous object
                    u.Id,
                    Username = u.UserName,
                    IsDisabled = u.UserProfile.isDisabled,
                    PhotoUrl = u.UserProfile.Photos.FirstOrDefault(p => p.IsMain).Url,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{userId}")]
        public async Task<ActionResult> EditRoles(int userId, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToList();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null) return BadRequest("No such user found");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("disable-account/{userId}")]
        public async Task<ActionResult> DisableAccount(int userId, [FromQuery] string isDisabled)
        {
            var userProfile = await _unitOfWork.UserProfileRepository.GetUserByAppIdAsync(userId);

            if (userProfile is null) return BadRequest("No such user is found");

            if (string.IsNullOrEmpty(isDisabled)) return BadRequest("You have not specified a option");

            if (isDisabled == "true") userProfile.isDisabled = true;

            if (isDisabled == "false") userProfile.isDisabled = false;

            _unitOfWork.UserProfileRepository.Update(userProfile);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest();
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPhotosForModerations()
        {
            return Ok(await _unitOfWork.PhotoRepository.GetUnapprovedPhoto());
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("approve-photo/{photoId}")]
        public async Task<ActionResult> ApprovePhoto(int photoId)
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(photoId);

            if (photo is null) return NotFound("could not find photo");

            photo.IsApprove = true;

            var user = await
            _unitOfWork.UserProfileRepository.GetUserByPhotoId(photoId);
            if (!user.Photos.Any(x => x.IsMain)) photo.IsMain = true;

            await _unitOfWork.Complete();
            return Ok();
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("reject-photo/{photoId}")]
        public async Task<ActionResult> RejectPhoto(int photoId)
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(photoId);

            if (photo.PublicId is not null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Result == "ok") _unitOfWork.PhotoRepository.RemovePhoto(photo);
            }
            else
            {
                _unitOfWork.PhotoRepository.RemovePhoto(photo);
            }

            await _unitOfWork.Complete();
            return Ok();
        }
    }
}

