namespace Room8.Domain.Entities
{
    public class SavedApartment : IAuditable
    {
        //public int SavedPropertyId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public long ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
        public DateTime SavedDate { get; set; }


        public long Id { get; set; }
        //public string Name { get; set; } = "";
       // public List<Apartment> Apartments { get; set; } = new List<Apartment>();

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
