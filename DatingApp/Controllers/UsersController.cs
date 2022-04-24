using System.Security.Claims;
using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Extensions;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Controllers
{    
    [Authorize]
    public class UsersController : BaseController
    {        
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserProfileRepository userProfileRepository, IMapper mapper, IPhotoService photoService)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        // api/users
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetUsers()
        {
           return Ok( await _userProfileRepository.GetUserProfilesAsync());
        }

        //api/user/2
        [HttpGet("{id}", Name = "GetUser")]        
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

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {                                  
            var userProfile = await _userProfileRepository.GetUserByUserNameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file); 

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (userProfile.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            userProfile.Photos.Add(photo);

            if (await _userProfileRepository.SaveAllAsync())
            {
                // this will include the location in headers where you get the photos
                return CreatedAtRoute("GetUser" , new {id = userProfile.Id}, _mapper.Map<PhotoDto>(photo));
            } 

            return BadRequest("Problem Adding Photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var userProfile = await _userProfileRepository.GetUserByUserNameAsync(User.GetUsername());

            var photo = userProfile.Photos.SingleOrDefault(p => p.Id == photoId);

            if(photo.IsMain) return  BadRequest("This Photo is already a main photo");

            var currentMainPhoto = userProfile.Photos.SingleOrDefault(p => p.IsMain);

            if (currentMainPhoto != null) currentMainPhoto.IsMain = false;
            
            photo.IsMain = true;

            if (await _userProfileRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var userProfile = await _userProfileRepository.GetUserByUserNameAsync(User.GetUsername());
            var photo = userProfile.Photos.SingleOrDefault(p => p.Id == photoId);

            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("Cannot Delete Main Photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            userProfile.Photos.Remove(photo);
            if (await _userProfileRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to delete the photo");
        }
    }
}