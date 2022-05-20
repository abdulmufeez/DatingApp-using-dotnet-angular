using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs
{
    public class UserProfileCreateDto
    {         
        public string FirstName { get; set; }        
        [Required] public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }     
        [Required] public string KnownAs { get; set; }        
        [Required] public string Gender { get; set; }        
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string City { get; set; }
        public int ApplicationUserId { get; set; }

    }
}