using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
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
        public async Task<ActionResult<SignedInUserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is already taken");
        
            using var hmac = new HMACSHA512();

            var user = new ApplicationUser()
            {
                UserName = registerDto.Username.ToLower(),
                HashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                SaltPassword = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new SignedInUserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApplicationUser>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(user => user.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.SaltPassword);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.HashedPassword[i]) return Unauthorized("Invalid Password");
            }

            return user;
        }
        private async Task<bool> UserExists(string username) 
            => await _context.Users.AnyAsync(user => user.UserName == username.ToLower());
   }
}