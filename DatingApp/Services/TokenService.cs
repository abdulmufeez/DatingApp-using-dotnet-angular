using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.Services
{
    //Creating Logic for JWT
    // Asp Mvc we use antiforgerytoken 
    //which automatically maintain every security communication between client and server
    public class TokenService : ITokenService
    {    
        private readonly SymmetricSecurityKey _key;
        //same key for both cient and server used in token verifications
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                //new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };
            //these portion is for which things can an application claims to server

            var signInCreds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);
            //Creating creddentials place in JWT token

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = signInCreds
            };
            //things in token and how it look like

            var tokenHandler = new JwtSecurityTokenHandler();
            //it is for reading and ceating tokens, initilizing token basically

            var token = tokenHandler.CreateToken(tokenDescriptor);
            //token creation

            return tokenHandler.WriteToken(token);
            //sending written token
        }
    }
}