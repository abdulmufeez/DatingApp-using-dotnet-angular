using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{    
    [Authorize]
    public class UsersController : BaseController
    {        
        private readonly IUserProfileRepository _userProfileRepository;

        public UsersController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        // api/users
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetUsers()
        {
            return Ok(await _userProfileRepository.GetUserProfilesAsync());
        }

        //api/user/2
        [HttpGet("{knownas}")]        
        public async Task<ActionResult<UserProfile>> GetUser(string knownas) 
        {            
            return await _userProfileRepository.GetUserProfileByUserNameAsync(knownas);
        }
    }
}