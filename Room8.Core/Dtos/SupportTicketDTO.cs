using Room8.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Core.Dtos
{
    public class SupportTicketDTO
    {
        [Required]
        public string TicketTitle { get; set; } = "";
        [Required ]
        public string TicketDescription { get; set; } = "";
       
        
       
    }
}
