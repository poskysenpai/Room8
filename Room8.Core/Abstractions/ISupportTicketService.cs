using Room8.Core.Dtos;
using Room8.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Core.Abstractions
{
    public interface ISupportTicketService
    {
        Task<ResponseDto<PaginatorDto<IEnumerable<SupportTicketResponseDto>>>> GetAllSupportTickets(int pageNumber, int pageSize);
        Task<ResponseDto<TicketStatusResponse>> GetSupportTicket(long Id);
        Task<ResponseDto<SupportTicket>> UpdateStatus(long ticketId);
        Task<ResponseDto<SupportTicketDTO>> CreateSupportTicket(SupportTicketDTO supportTicket);
    }
}
