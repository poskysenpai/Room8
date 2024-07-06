using Microsoft.AspNetCore.Identity;

namespace Room8.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string? ProfilePictureUrl { get; set; } 
        public int Age { get; set; }
        public string? BirthDate { get; set; } 
        public string? City { get; set; } 
        public string? Gender { get; set; } 
        public string? Industry { get; set; } 
        public string? Occupation { get; set; } 
        public string? ZodiacSign { get; set; } 
        public string? Location { get; set; }
        public string? HowDidYouHear { get; set; }
        //navigation to apartment
        public long ApartmentId { get; set; }
        public List<Apartment> Apartments { get; set; } = new List<Apartment>();
        //public List<Message> Messages { get; set; } = new List<Message>();
        public List<UserChatRoom> UserChatRooms { get; set; } = new List<UserChatRoom>();
        public List<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
        //public List<SavedApartment> SavedApartments { get; set; } = new List<SavedApartment>();
        //public List<RentShare> RentShares { get; set; } = new List<RentShare>();
        //public List<Tenant> Tenants { get; set; } = new List<Tenant>();
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; } 
        public bool IsDeleted { get; set; }
    }
}
