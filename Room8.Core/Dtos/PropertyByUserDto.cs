using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Core.Dtos
{
    public class PropertyByUserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int NoOfRooms { get; set; }
        public string? ImageUrl { get; set; }
    }
}
