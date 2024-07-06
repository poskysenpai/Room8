namespace Room8.Domain.Entities
{
    public class RentShare : IAuditable
    {
        public int ListingId { get; set; }
        public Listing Listing { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public decimal RentAmount { get; set; }
        public long Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
