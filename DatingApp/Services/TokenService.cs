using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.Services
{
    //Creating Logic for JWT
    // Asp Mvc we use antiforgerytoken 
    //which automatically maintain every security communication between client and server
    public class TokenService : ITokenService
    {    
        //same key for both cient and server used in token verifications
        private readonly SymmetricSecurityKey _key;        
        private readonly UserManager<ApplicationUser> _userManager;
        public TokenService(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public async Task<string> CreateToken(ApplicationUser user)
        {
            //these portion is for which things can an application claims to server 
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            // Adding role to the claim
            var roles =await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            //Creating creddentials place in JWT token
            var signInCreds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);

            //things in token and how it look like
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = signInCreds
            };
            
            //it is for reading and ceating tokens, initilizing token basically
            var tokenHandler = new JwtSecurityTokenHandler();            

            //token creation
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //sending written token
            return tokenHandler.WriteToken(token);
        }
    }
}