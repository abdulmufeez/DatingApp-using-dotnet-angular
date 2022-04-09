using AutoMapper;
using DatingApp.DTOs;
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
        private readonly IMapper _mapper;

        public UsersController(IUserProfileRepository userProfileRepository, IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        // api/users
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetUsers()
        {
            var usersProfiles = await _userProfileRepository.GetUserProfilesAsync();
            return Ok(_mapper.Map<IEnumerable<UserProfileDto>>(usersProfiles));
        }

        //api/user/2
        [HttpGet("{knownas}")]        
        public async Task<ActionResult<UserProfileDto>> GetUser(string knownas) 
        {            
            var userProfile = await _userProfileRepository.GetUserProfileByUserNameAsync(knownas);
            return _mapper.Map<UserProfileDto>(userProfile);
        }
    }
}