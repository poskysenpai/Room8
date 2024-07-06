using Room8.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Core.Dtos
{
    public class PropertyResponseDto
    {
        public int ListingId { get; set; }
        public decimal Price { get; set; }
        public int NumberOfRooms { get; set; }
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
        public List<RentShare> RentShares { get; set; } = new List<RentShare>();
    }
}
