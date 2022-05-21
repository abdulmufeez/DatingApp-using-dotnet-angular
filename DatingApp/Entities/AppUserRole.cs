using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public ApplicationUser User { get; set; }
        public AppRole Role { get; set; }
    }
}