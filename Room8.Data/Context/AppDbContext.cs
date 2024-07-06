using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Room8.Domain.Entities;

namespace Room8.Data.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //public DbSet<ProductMetrics> ProductMetrics { get; set; }
        public DbSet<Apartment> Apartments { get; set; }

        public DbSet<ApartmentImage> ApartmentImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<Listing> Listings { get; set; }
        //public DbSet<RentShare> RentShares { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<UserChatRoom> UserChatRooms { get; set; }
        public DbSet<SavedApartment> SavedApartments { get; set; }
        //public DbSet<Tenant> Tenants { get; set; }
        //public DbSet<UserAnalytics> UserAnalytics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Apartment>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<ApartmentImage>()
               .HasKey(a => a.Id);

            modelBuilder.Entity<Category>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Message>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<SavedApartment>()
                .HasKey(a => a.Id);


            // Configure Apartment - Category relationship
            //modelBuilder.Entity<Apartment>()
            //    .HasOne(p => p.Category)
            //    .WithMany(c => c.Apartments)
            //    .HasForeignKey(p => p.CategoryId);

            // Configure Property - User (Owner) relationship
            modelBuilder.Entity<Apartment>()
                 .HasOne(p => p.User)
                 .WithMany(u => u.Apartments)
                 .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<ApartmentImage>()
                .HasOne(p => p.Apartment)
                .WithMany(u => u.ApartmentImages)
                .HasForeignKey(p => p.ApartmentId);

            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserChatRooms)
                .WithOne(uc => uc.User)
                .HasForeignKey(uc => uc.UserId);

            // ChatRoom Configuration
            modelBuilder.Entity<ChatRoom>()
                .HasOne(cr => cr.User1)
                .WithMany()
                .HasForeignKey(cr => cr.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatRoom>()
                .HasOne(cr => cr.User2)
                .WithMany()
                .HasForeignKey(cr => cr.User2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatRoom>()
                .HasMany(cr => cr.Messages)
                .WithOne(m => m.ChatRoom)
                .HasForeignKey(m => m.ChatRoomId);

            // Message Configuration
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserChatRoom Configuration
            modelBuilder.Entity<ChatRoom>()
             .HasMany(cr => cr.UserChatRooms)
             .WithOne(ucr => ucr.ChatRoom)
             .HasForeignKey(ucr => ucr.ChatRoomId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserChatRoom>()
                .HasKey(ucr => new { ucr.UserId, ucr.ChatRoomId });

            modelBuilder.Entity<UserChatRoom>()
                .HasOne(ucr => ucr.User)
                .WithMany(u => u.UserChatRooms)
                .HasForeignKey(ucr => ucr.UserId);

            modelBuilder.Entity<UserChatRoom>()
                .HasOne(ucr => ucr.ChatRoom)
                .WithMany(cr => cr.UserChatRooms)
                .HasForeignKey(ucr => ucr.ChatRoomId);

            // Configure Listing - Property relationship
            //modelBuilder.Entity<Listing>()
            //    .HasOne(l => l.Apartment)
            //    .WithMany(p => p.Listings)
            //    .HasForeignKey(l => l.ApartmentId);

            // Configure RentShare - Listing relationship
            //modelBuilder.Entity<RentShare>()
            //    .HasOne(rs => rs.Listing)
            //    .WithMany(l => l.RentShares)
            //    .HasForeignKey(rs => rs.ListingId);

            // Configure RentShare - User relationship
            //modelBuilder.Entity<RentShare>()
            //    .HasOne(rs => rs.User)
            //    .WithMany(u => u.RentShares)
            //    .HasForeignKey(rs => rs.UserId);

            // Configure Message relationships
            //modelBuilder.Entity<Message>()
            //    .HasOne(m => m.Sender)
            //    .WithMany(u => u.SentMessages)
            //    .HasForeignKey(m => m.SenderId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Message>()
            //    .HasOne(m => m.Receiver)
            //    .WithMany(u => u.ReceivedMessages)
            //    .HasForeignKey(m => m.ReceiverId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // Configure SavedApartment relationships
            //modelBuilder.Entity<SavedApartment>()
            //    .HasOne(sa => sa.User)
            //    .WithMany(u => u.SavedApartments)
            //    .HasForeignKey(sa => sa.UserId);


            //Configure SupportTicket relationships
            modelBuilder.Entity<SupportTicket>()
                .HasOne(st => st.User)
                .WithMany(u => u.SupportTickets)
                .HasForeignKey(st => st.UserId);

            // Configure Tenant relationships
            //modelBuilder.Entity<Tenant>()
            //    .HasOne(t => t.User)
            //    .WithMany(u => u.Tenants)
            //    .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsDeleted = false,
                    Name = "Duplex",
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now
                } 
            );




        }
    }
}
