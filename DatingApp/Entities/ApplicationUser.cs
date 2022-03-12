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
        public string FirstName { get; set; }
        public string LastName { get; set; }                
        public string UserName { get; set; } = null!;       //= null! is used for not null feild
        public string Email { get; set; }
        public byte[] HashedPassword { get; set; }
        public byte[] SaltPassword { get; set; }
    }
}