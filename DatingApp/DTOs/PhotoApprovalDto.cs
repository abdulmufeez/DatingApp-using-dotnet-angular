namespace DatingApp.DTOs
{
    public class PhotoApprovalDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsApproved { get; set; }
        public string KnownAs { get; set; }        
        public int UserProfileId { get; set; }
    }
}