namespace DatingApp.Entities
{
    public class Message
    {
        public int Id { get; set; }

        public UserProfile Sender { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }

        public UserProfile Recipient { get; set; }
        public int RecipientId { get; set; }
        public string RecipientName { get; set; }

        public string Content { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.Now; 
        public DateTime? MessageRead { get; set; }

        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
        public bool SenderDeletedBoth { get; set; }        
    }
}