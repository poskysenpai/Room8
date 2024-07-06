namespace Room8.Domain.Entities
{
    public class Category : IAuditable
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        //public List<Apartment>? Apartments { get; set; } = new List<Apartment>();

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}