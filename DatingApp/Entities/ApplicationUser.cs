using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Entities
{
    public class ApplicationUser
    {
        public int Id { get; set; }                        
        public string UserName { get; set; } = null!;       //= null! is used for not null feild

        [Required]
        public string Email { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public byte[] HashedPassword { get; set; }
        public byte[] SaltPassword { get; set; }        
    }
}