using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Core.Dtos
{
    public class AptRequestDTO
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; } = "";
        public string? Features { get; set; } = "";

        public string Address { get; set; } = "";
        public string Location { get; set; } = "";
        public List<IFormFile> ImageUrls { get; set; }

        public string TypeOfUnit { get; set; } = "";
        public long? CategoryId { get; set; }
        public decimal Price { get; set; }
        public int NumberOfRooms { get; set; }
    }
}
