using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Domain.Entities
{
    public class ApartmentImage : IAuditable
    {
        public string ImageUrl { get; set; }

        public string PublicId { get; set; }

        public Apartment Apartment { get; set; }

        public long ApartmentId { get; set; }
        public long Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
