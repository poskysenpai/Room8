using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Room8.Core.Dtos;
using Room8.Domain.Entities;
using Room8.Infrastructure.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Room8.Core.Implementations
{
    public class ChatHub : Hub
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<ChatRoom> _chatRoomRepository;
        private readonly IRepository<UserChatRoom> _userChatRoomRepository;
        private readonly UserManager<User> _userManager;

        public ChatHub(IRepository<Message> messageRepository, IRepository<ChatRoom> chatRoomRepository,
            IRepository<UserChatRoom> userChatRoomRepository, UserManager<User> userManager)
        {
            _messageRepository = messageRepository;
            _chatRoomRepository = chatRoomRepository;
            _userChatRoomRepository = userChatRoomRepository;
            _userManager = userManager;
        }

        public async Task StartChat(string user1Id, string user2Id)
        {
            var user1 = await _userManager.FindByIdAsync(user1Id);
            var user2 = await _userManager.FindByIdAsync(user2Id);

            if (user1 == null || user2 == null)
            {
                throw new Exception("One or both users do not exist.");
            }

            var chatRoom = _chatRoomRepository.FindByConditionAsNoTracking(cr =>
                (cr.User1Id == user1Id && cr.User2Id == user2Id) ||
                (cr.User1Id == user2Id && cr.User2Id == user1Id)).SingleOrDefault();

            using (var transactionObject = await _chatRoomRepository.GetTransactionObject())
            {
                try
                {
                    if (chatRoom == null)
                    {
                        chatRoom = new ChatRoom
                        {
                            User1Id = user1Id,
                            User2Id = user2Id
                        };
                        var user1ChatRoom = new UserChatRoom
                        {
                            UserId = user1Id,
                            ChatRoomId = chatRoom.Id,
                        };
                        var user2ChatRoom = new UserChatRoom
                        {
                            UserId = user2Id,
                            ChatRoomId = chatRoom.Id,
                        };

                        chatRoom.UserChatRooms.Add(user1ChatRoom);
                        chatRoom.UserChatRooms.Add(user2ChatRoom);

                        await _chatRoomRepository.CreateAsync(chatRoom);

                        await transactionObject.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    await transactionObject.RollbackAsync();
                    throw ex;
                }
            }
            

            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoom.Id.ToString());
            await Clients.Group(chatRoom.Id.ToString()).SendAsync("ReceiveNotification", $"{user1.UserName} and {user2.UserName} are now in a chat.");

            // Notify the user who started the chat about the new chat room
            await Clients.User(user1Id).SendAsync("ChatStarted", new
            {
                Id = chatRoom.Id,
                User1Id = user1Id,
                User2Id = user2Id
            });
        }

        public async Task SendMessage(string senderId, string receiverId, string senderName, string messageBody)
        {
            var chatRoom = _chatRoomRepository.FindByCondition(cr =>
                (cr.User1Id == senderId && cr.User2Id == receiverId) ||
                (cr.User1Id == receiverId && cr.User2Id == senderId)).SingleOrDefault();

            if (chatRoom == null)
            {
                throw new Exception("Chat room does not exist.");
            }

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                MessageBody = messageBody,
                CreatedAt = DateTime.UtcNow,
                ChatRoomId = chatRoom.Id
            };

            await _messageRepository.CreateAsync(message);

            var msgResponse = new MessageResponse
            {
                SenderId = senderId,
                SenderName = senderName,
                SenderProfileUrl = "",
                ReceiverId = receiverId,
                MessageBody = messageBody,
                CreatedAt = MessageService.FormatTo12Hour(DateTime.UtcNow), // client should adjust to user's local time
                ChatRoomId = chatRoom.Id,
                IsRead = false,
                Id = message.Id,
            };

            await Clients.Group(chatRoom.Id.ToString()).SendAsync("ReceiveMessage", msgResponse);
        }
    }
}
