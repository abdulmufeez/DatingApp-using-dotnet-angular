using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Extensions;

namespace DatingApp.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        [Required]
        public string KnownAs { get; set; }
        public DateOnly ProfileCreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateTime LastActive { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        public ICollection<Photo> Photos { get; set; }  

        public ApplicationUser ApplicationUser { get; set; }      
        public int ApplicationUserId { get; set; }

        public int GetAge(DateOnly dob){
            return DateOfBirth.CalculateAge();
        }
    }
}