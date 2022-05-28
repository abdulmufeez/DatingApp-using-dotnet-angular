using Microsoft.AspNetCore.Identity;

namespace DatingApp.Entities
{
    // by default identity use id as a string so, we have to explicitly tell it to use int by <int>
    public class ApplicationUser : IdentityUser<int>
    {        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public UserProfile UserProfile { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }    
    }    
}