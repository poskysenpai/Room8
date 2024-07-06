
namespace Room8.Domain.Entities
{
    public class SupportTicket : IAuditable
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string TicketTitle { get; set; }
        public string TicketDescription { get; set; }
        public string Status { get; set; } // Open, In Progress, Resolved
        public long Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class SupportTicketStatus
    {
        public const string Open = "Open";
        public const string InProgress = "In Progress";
        public const string Resolved = "Resolved";

    }
}