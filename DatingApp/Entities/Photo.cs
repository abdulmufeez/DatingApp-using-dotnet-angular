using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }        
        public bool IsMain { get; set; }
        public string PublicId { get; set; }


        public UserProfile UserProfile { get; set; }
        public int UserProfileId { get; set; }
    }
}