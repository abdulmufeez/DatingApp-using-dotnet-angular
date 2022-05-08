using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Extensions;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is already taken");
            if (await UserEmailExists(registerDto.Email)) return BadRequest("Email is already taken");

            using var hmac = new HMACSHA512();

            var user = new ApplicationUser()
            {
                UserName = registerDto.Username.ToLower(),
                Email = registerDto.Email,
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                SaltPassword = hmac.Key
            };

            _context.ApplicationUser.Add(user);
            await _context.SaveChangesAsync();

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
            var user = await _context.ApplicationUser.SingleOrDefaultAsync(user => user.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.SaltPassword);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.HashedPassword[i]) return Unauthorized("Invalid Password");
            }

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
            => await _context.ApplicationUser.AnyAsync(user => user.UserName == username.ToLower());

        private async Task<bool> UserEmailExists(string email)
            => await _context.ApplicationUser.AnyAsync(user => user.Email == email);
    }
}