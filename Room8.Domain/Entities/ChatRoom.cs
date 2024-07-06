using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Domain.Entities
{
    public class ChatRoom
    {
        public long Id { get; set; }
        public string User1Id { get; set; }
        public User User1 { get; set; }
        public string User2Id { get; set; }
        public User User2 { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
        public List<UserChatRoom> UserChatRooms { get; set; } = new List<UserChatRoom>();
    }
}
