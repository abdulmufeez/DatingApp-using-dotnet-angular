namespace DatingApp.Helpers
{
    public class LikeParams : PaginationParams
    {
        public int UserProfileId { get; set; }
        public string Predicate { get; set; }
    }
}