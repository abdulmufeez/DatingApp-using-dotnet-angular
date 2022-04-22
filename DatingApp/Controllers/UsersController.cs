using System.Security.Claims;
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
           return Ok( await _userProfileRepository.GetUserProfilesAsync());
        }

        //api/user/2
        [HttpGet("{id}")]        
        public async Task<ActionResult<UserProfileDto>> GetUser(int id) 
        {            
            return await _userProfileRepository.GetUserProfileByIdAsync(id);
        }

        [HttpGet("edit/{id}")]
        public async Task<ActionResult<UserProfileDto>> GetUserByAppId(int id){
            return await _userProfileRepository.GetUserProfileByAppIdAsync(id);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UserProfileUpdateDto userProfileUpdateDto){
            var user = await _userProfileRepository.GetUserByIdAsync(userProfileUpdateDto.Id);

            _mapper.Map(userProfileUpdateDto, user);

            _userProfileRepository.Update(user);

            if (await _userProfileRepository.SaveAllAsync()) return NoContent();

            return BadRequest(); 
        }
    }
}