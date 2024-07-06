using Room8.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Core.Abstractions
{
    public interface IMessageService
    {
        Task<ResponseDto<IEnumerable<MessageResponse>>> GetMessages(long chatRoomId);
    }
}
