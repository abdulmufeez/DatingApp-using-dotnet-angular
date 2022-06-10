namespace DatingApp.Entities
{
    public class Connection
    {
        // default constructor is must need to initilize otherwise entity framework give error
        public Connection()
        {
        }

        public Connection(string connectionId, int userProfileId)
        {
            ConnectionId = connectionId;
            UserProfileId = userProfileId;
        }

        public string ConnectionId { get; set; }
        public int UserProfileId { get; set; }
    }
}