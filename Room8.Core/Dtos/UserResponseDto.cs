using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Core.Dtos
{
    public class UserResponseDto
    {
        public string Id { get; set; } = "";
        public string FullName { get; set; } = "";
        public int Age { get; set; }
        public string Gender { get; set; } = "";
        public DateTimeOffset CreatedAt { get; set; }
        public string ApartmentName { get; set; } = "";
    }
}
