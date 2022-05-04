namespace DatingApp.Entities
{
    public class UserLike
    {
        // the one who give like
        public UserProfile SourceUser { get; set; }
        public int SourceUserId { get; set; }

        // the one who is getting liked
        public UserProfile LikedUser { get; set; }
        public int LikedUserId { get; set; }
    }
}