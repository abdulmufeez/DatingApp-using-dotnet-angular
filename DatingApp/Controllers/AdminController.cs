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
        private readonly IUserProfileRepository _userProfileRepository;

        public AdminController(UserManager<ApplicationUser> userManager,
            IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
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
                .Select(u => new                // Select project anything to request response means sends 
                {                               // new {} => this will create an anonymous object
                    u.Id,                    
                    Username = u.UserName,
                    isDisabled = u.UserProfile.isDisabled,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()                    
                })
                .ToListAsync();            

            return Ok(users);
        }

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

        [HttpPut("disable-account/{userId}")]
        public async Task<ActionResult> DisableAccount(int userId, [FromQuery] string isDisabled)
        {
            var userProfile = await _userProfileRepository.GetUserByAppIdAsync(userId);

            if (userProfile is null) return BadRequest("No such user is found");

            if (string.IsNullOrEmpty(isDisabled)) return BadRequest("You have not specified a option");

            if (isDisabled == "true") userProfile.isDisabled = true;

            if (isDisabled == "false") userProfile.isDisabled = false;

            _userProfileRepository.Update(userProfile);

            if (await _userProfileRepository.SaveAllAsync()) return NoContent();

            return BadRequest();
        }
    }
}

