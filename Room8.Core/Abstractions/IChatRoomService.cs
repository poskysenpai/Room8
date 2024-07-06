using Room8.Core.Dtos;

namespace Room8.Core.Abstractions
{
    public interface IChatRoomService
    {
        Task<ResponseDto<IEnumerable<ChatRoomResponse>>> GetChatRooms(string userId);
    }
}
