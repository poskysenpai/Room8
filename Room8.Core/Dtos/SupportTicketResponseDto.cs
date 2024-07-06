using Room8.Core.Utilities;
using Room8.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Core.Dtos
{
    public class SupportTicketResponseDto
    {
        public long Id { get; set; }
        public string UserId { get; set; }    
        public string TicketTitle { get; set; }
        public string TicketDescription { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Status { get; set; }
    }
}
