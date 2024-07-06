using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Core.Dtos
{
    public class SavedApartmentResponse
    {

        public long ApartmentId { get; set; }

        public long Id { get; set; }

        public string UserId { get; set; }
        public DateTime SavedDate { get; set; }
    }
}
