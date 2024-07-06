using Microsoft.EntityFrameworkCore;
using Room8.Core.Abstractions;
using Room8.Core.Dtos;
using Room8.Domain.Entities;
using Room8.Infrastructure.Abstractions;

namespace Room8.Core.Implementations
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly IRepository<UserChatRoom> _userChatRoomRepository;

        public ChatRoomService(
            IRepository<UserChatRoom> userChatRoomRepository)
        {
            _userChatRoomRepository = userChatRoomRepository;
        }
        public async Task<ResponseDto<IEnumerable<ChatRoomResponse>>> GetChatRooms(string userId)
        {
            var chatRooms = _userChatRoomRepository.FindByCondition(x => x.UserId == userId)
                .Include(x => x.ChatRoom)
                    .ThenInclude(cr => cr.User1)
                .Include(x => x.ChatRoom)
                    .ThenInclude(cr => cr.User2)
                .Select(x => MapToChatRoomResponse(x.ChatRoom))
                .ToList();
            return ResponseDto<IEnumerable<ChatRoomResponse>>.Success(chatRooms, "Successfully gotten chat rooms", 200);
        }

        public static ChatRoomResponse MapToChatRoomResponse(ChatRoom chatRoom)
        {
            return new ChatRoomResponse()
            {
                Id = chatRoom.Id,
                User1Id = chatRoom.User1Id,
                User1Name = $"{chatRoom.User1.FirstName} {chatRoom.User1.LastName}",
                User1ProfileUrl = chatRoom.User1.ProfilePictureUrl,
                User2Id = chatRoom.User2Id,
                User2Name = $"{chatRoom.User2.FirstName} {chatRoom.User2.LastName}",
                User2ProfileUrl = chatRoom.User2.ProfilePictureUrl
            };
        }
    }
}
