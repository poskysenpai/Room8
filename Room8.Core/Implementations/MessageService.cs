using Microsoft.EntityFrameworkCore;
using Room8.Core.Abstractions;
using Room8.Core.Dtos;
using Room8.Domain.Entities;
using Room8.Infrastructure.Abstractions;
using Room8.Core.Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Room8.Core.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> _messageRepository;
        public MessageService(IRepository<Message> messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<ResponseDto<IEnumerable<MessageResponse>>> GetMessages(long chatRoomId)
        {
            var messages = _messageRepository.FindByCondition(x => x.ChatRoomId == chatRoomId)
                .Include(x => x.Sender).Include(x => x.Receiver).OrderBy(x => x.CreatedAt).Select(MapToMessageResponse);

            return ResponseDto<IEnumerable<MessageResponse>>.Success(messages, "Successfully gotten Messages", 200);
        }

        public static MessageResponse MapToMessageResponse(Message message)
        {
            return new MessageResponse
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderName = $"{message.Sender.FirstName} {message.Sender.LastName}",
                SenderProfileUrl = message.Sender.ProfilePictureUrl,
                ReceiverId = message.ReceiverId,
                ReceiverName = $"{message.Receiver.FirstName} {message.Receiver.LastName}",
                MessageBody = message.MessageBody,
                ChatRoomId = message.ChatRoomId,
                IsRead = message.IsRead,
                CreatedAt = FormatTo12Hour(message.CreatedAt)
            };
        }

        public static string FormatTo12Hour(DateTimeOffset date)
        {
            int hours = date.Hour;
            int minutes = date.Minute;

            var ampm = hours >= 12 ? "PM" : "AM";

            hours %= 12;
            hours = hours != 0 ? hours : 12;

            var minutesStr = minutes < 10 ? '0' + minutes : minutes;

            var formattedTime = $"{hours}:{minutesStr} {ampm}";

            return formattedTime;
        }
    }
}
