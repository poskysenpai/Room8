using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Domain.Entities
{
    public class UserChatRoom
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public long ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}
