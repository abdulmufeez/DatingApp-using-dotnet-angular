using Microsoft.AspNetCore.Identity;

namespace DatingApp.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public ApplicationUser User { get; set; }
        public AppRole Role { get; set; }
    }
}