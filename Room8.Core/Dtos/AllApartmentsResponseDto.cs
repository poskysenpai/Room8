using Room8.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Core.Dtos
{
    public class AllApartmentsResponseDto
    {
        public long Id { get; set; }
        public string? Name { get; set; } = "";
        public string? Description { get; set; } = "";
        public string? Address { get; set; } = "";
        public string? Location { get; set; } = "";
        public string? ImageUrl { get; set; } 
        public string? VideoUrl { get; set; } 
        public decimal? Price { get; set; }
        public int? NumberOfRooms { get; set; }
        public string? Features { get; set; } 
        public string? CategoryName { get; set; }
        public bool? IsAvailable { get; set; }
        public string? OwnerId { get; set; }
        public string? OwnerName { get; set; }
        public bool? IsSaved { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
