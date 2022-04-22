namespace DatingApp.DTOs
{
    public class UserProfileUpdateDto 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string KnownAs { get; set; }        
        public string Gender { get; set; }        
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }        
        public string Country { get; set; }        
        public string City { get; set; }
        public int ApplicationUserId { get; set; }
    }
}