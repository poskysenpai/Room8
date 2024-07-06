namespace Room8.Domain.Entities
{
    public class Tenant
    {
        public int TenantId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Age { get; set; }
        public string Gender { get; set;}
        public DateTime DateCreated { get; set; }
        public List<RentShare> RentShares { get; set; } = new List<RentShare>();
    }
}
