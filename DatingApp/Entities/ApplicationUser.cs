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

        [Required]
        public string UserName { get; set; } = null!;

        public byte[] HashedPassword { get; set; }
        public byte[] SaltPassword { get; set; }
    }
}