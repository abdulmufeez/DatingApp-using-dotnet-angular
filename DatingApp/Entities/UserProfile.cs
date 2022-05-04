using System.ComponentModel.DataAnnotations;

namespace DatingApp.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string KnownAs { get; set; }
        public DateTime ProfileCreatedAt { get; set; } = DateTime.Now;
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

        public ICollection<UserLike> LikedByUsers { get; set; }         
        public ICollection<UserLike> LikedUsers { get; set; }

        // automapper automatically use this function and assign int value to the property
        // public int GetAge(){
        //     return DateOfBirth.CalculateAge();
        // }
    }
}