using Room8.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Core.Dtos
{
    public class MessageResponse
    {
        public string SenderId { get; set; } = "";
        public string SenderName { get; set; } = "";
        public string SenderProfileUrl { get; set; } = "";
        public string ReceiverId { get; set; } = "";
        public string ReceiverName { get; set; } = "";
        public string MessageBody { get; set; } = "";
        public long ChatRoomId { get; set; }
        public bool IsRead { get; set; }
        public long Id { get; set; }
        public string CreatedAt { get; set; } = "";
    }
}
