 

namespace Room8.Domain.Entities
{
    public class Apartment : IAuditable
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Address { get; set; } = "";
        public string Location { get; set; } = "";
        public List<ApartmentImage> ApartmentImages { get; set; } 
        public string VideoUrl { get; set; } = "";
        public decimal Price { get; set; }
        public int NumberOfRooms { get; set; }
        public string Features { get; set; } = "";
        public long? CategoryId { get; set; }
        public Category? Category { get; set; }
        public bool IsAvailable { get; set; }


        public string? UserId { get; set; }
        public User User { get; set; }
        //public List<Listing> Listings { get; set; } = new List<Listing>();
        public bool IsSaved { get; set; }


        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
