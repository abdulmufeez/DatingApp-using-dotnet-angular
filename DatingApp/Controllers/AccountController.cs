using AutoMapper;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Extensions;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly DataContext _context;  
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, DataContext context, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager; 
            _context = context;
            _mapper = mapper;           
            _tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is already taken");
            if (await UserEmailExists(registerDto.Email)) return BadRequest("Email is already taken");

            var user = _mapper.Map<ApplicationUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return new ApplicationUserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApplicationUserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(user => user.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid Username"); 

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Invalid password");           

            var userProfile = await _context.UserProfile
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(m => m.ApplicationUserId == user.Id);

            if (userProfile == null)
            {
                return new ApplicationUserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    PhotoUrl = null,
                    Token = _tokenService.CreateToken(user)
                };
            }

            return new ApplicationUserDto
            {
                Id = user.Id,
                Username = user.UserName,
                PhotoUrl = userProfile.Photos.SingleOrDefault(p => p.IsMain)?.Url,
                Token = _tokenService.CreateToken(user),
                Gender = userProfile.Gender,
                KnownAs = userProfile.KnownAs,
                Age = userProfile.DateOfBirth.CalculateAge()
            };
        }

        //checking if username is already exist
        private async Task<bool> UserExists(string username)
            => await _userManager.Users.AnyAsync(user => user.UserName == username.ToLower());

        private async Task<bool> UserEmailExists(string email)
            => await _userManager.Users.AnyAsync(user => user.Email == email);
    }
}