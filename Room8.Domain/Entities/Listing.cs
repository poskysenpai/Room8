namespace Room8.Domain.Entities
{
    public class Listing : IAuditable
    {
        
        public decimal Price { get; set; }
        public int NumberOfRooms { get; set; }
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }     
        public List<RentShare> RentShares { get; set; } = new List<RentShare>();
        public long Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}