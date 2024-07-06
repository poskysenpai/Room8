namespace Room8.Domain.Entities
{
    public class Message : IAuditable
    {
        public string SenderId { get; set; }
        public User Sender { get; set; }
        public string ReceiverId { get; set; }
        public User Receiver { get; set; }
        public string MessageBody { get; set; }
        public long ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
        public bool IsRead { get; set; }    
        public bool? IsDeleted { get; set; }
        public long Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
 
}
