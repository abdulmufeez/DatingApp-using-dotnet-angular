using DatingApp.Entities;

namespace DatingApp.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken (ApplicationUser user);
    }
}