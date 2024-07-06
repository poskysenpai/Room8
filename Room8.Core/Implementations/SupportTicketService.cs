using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Room8.Core.Abstractions;
using Room8.Core.Dtos;
using Room8.Data.Context;
using Room8.Core.Utilities;
using Room8.Domain.Entities;
using Room8.Infrastructure.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Room8.Core.Implementations
{
    public class SupportTicketService : ISupportTicketService
    {
        private readonly IRepository<SupportTicket> _supportTicketRepository;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SupportTicketService(IRepository<SupportTicket> supportTicketRepository, UserManager<User> userManager, AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _supportTicketRepository = supportTicketRepository;
            _userManager = userManager;
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDto<PaginatorDto<IEnumerable<SupportTicketResponseDto>>>> GetAllSupportTickets(int pageNumber, int pageSize)
        {
            var errors = new List<Error>();

            if (pageNumber <= 0 || pageSize <= 0)
            {
                errors.Add(new Error("400", "Page number and page size must be greater than zero."));
                return ResponseDto<PaginatorDto<IEnumerable<SupportTicketResponseDto>>>.Failure(errors, 400);
            }

            try
            {
                 
                var allSupportTickets = _supportTicketRepository.FindByCondition(x => true)
                               .Include(ticket => ticket.User)  
                               .AsQueryable();


                var supportTickets = allSupportTickets
                    .Where(ticket => !ticket.IsDeleted.HasValue || !ticket.IsDeleted.Value)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(ticket => new SupportTicketResponseDto
                    {
                        Id = ticket.Id,
                        UserId = ticket.UserId,
                        FirstName = ticket.User.FirstName,
                        LastName = ticket.User.LastName,
                        TicketTitle = ticket.TicketTitle,
                        TicketDescription = ticket.TicketDescription,
                        Status = ticket.Status,
                        CreatedAt = ticket.CreatedAt,
                        UpdatedAt = ticket.UpdatedAt
                    })
                    .ToList();

                var totalTickets = await allSupportTickets.CountAsync(ticket => !ticket.IsDeleted.HasValue || !ticket.IsDeleted.Value);

                var paginatorDto = new PaginatorDto<IEnumerable<SupportTicketResponseDto>>
                {
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    NumberOfPages = totalTickets,
                    PageItems = supportTickets

                };

                return ResponseDto<PaginatorDto<IEnumerable<SupportTicketResponseDto>>>.Success(paginatorDto, "Successfully fetched all support tickets", 200);
            }
            catch (Exception ex)
            {
                errors = new List<Error> { new Error("500", ex.Message) };
                return ResponseDto<PaginatorDto<IEnumerable<SupportTicketResponseDto>>>.Failure(errors, 500);
            }


        }


        public async Task<ResponseDto<TicketStatusResponse>> GetSupportTicket(long Id)
        {
            var errors = new List<Error>();
            try
            {
                var supportTicket = await _supportTicketRepository.FindByCondition(x => x.Id == Id).FirstOrDefaultAsync();
                if (supportTicket is null)
                {
                    errors.Add(new Dtos.Error("404", "Ticket not found"));
                    return ResponseDto<TicketStatusResponse>.Failure(errors, 404);
                }

                var foundTicket = new TicketStatusResponse
                {
                    Description = supportTicket.TicketDescription,
                    Status = supportTicket.Status,
                    UserID = supportTicket.UserId
                };
          
                return ResponseDto<TicketStatusResponse>.Success(foundTicket, "Ticket retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                errors = new List<Dtos.Error> { new Dtos.Error("500", ex.Message) };
                return ResponseDto<TicketStatusResponse>.Failure(errors, 500);
            }
        }

        public async Task<ResponseDto<SupportTicket>> UpdateStatus(long ticketId)
        {
            var ticket = await _appDbContext.SupportTickets.FindAsync(ticketId);
            if (ticket == null)
            {
                var error = new List<Error>
                {
                    new Error("404", "Ticket not found")
                };
                return ResponseDto<SupportTicket>.Failure(error);
            }

            try
            {

                if (ticket.Status == SupportTicketStatus.Open)
                {
                    ticket.Status = SupportTicketStatus.InProgress;
                }
                else if (ticket.Status == SupportTicketStatus.InProgress)
                {
                    ticket.Status = SupportTicketStatus.Resolved;
                }
                else
                {
                    var error = new List<Error>()
                    {
                        new Error ("400", "Invalid status transition")
                    };

                    return ResponseDto<SupportTicket>.Failure(error, 400);
                }
                _appDbContext.SupportTickets.Update(ticket);
                await _appDbContext.SaveChangesAsync();

                return ResponseDto<SupportTicket>.Success(ticket);

            }
            catch (Exception ex)
            {
                var error = new List<Error>
                {
                    new Error("500", $"Internal server error: {ex.Message}")
                };
                return ResponseDto<SupportTicket>.Failure(error);
            }
        }

        public async Task<ResponseDto<SupportTicketDTO>> CreateSupportTicket(SupportTicketDTO supportTicket)
        {
            var errors = new List<Error>();
            try
            {
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                if (user == null)
                {
                    errors.Add(new Dtos.Error("404", "Ticket not found"));
                    return ResponseDto<SupportTicketDTO>.Failure(errors, 404);
                }


                var newTicket = new SupportTicket
                {
                    TicketDescription = supportTicket.TicketDescription,
                    TicketTitle = supportTicket.TicketTitle,
                    Status = TicketStatus.Open,
                    UserId = user.Id,
                    User = user
                };
                await _supportTicketRepository.CreateAsync(newTicket);

                var dto = new SupportTicketDTO
                {
                    TicketDescription = newTicket.TicketDescription,
                    TicketTitle = newTicket.TicketTitle
                };

                return ResponseDto<SupportTicketDTO>.Success(dto, "Successfully Created a ticket", 201);


            }
            catch(Exception ex)
            {
                errors = new List<Dtos.Error> { new Dtos.Error("500", ex.Message) };
                return ResponseDto<SupportTicketDTO>.Failure(errors, 500);
            }

            //var ticket = new SupportTicketDTO
            //{
            //    TicketTitle = supportTicket.TicketTitle,
            //    TicketDescription = supportTicket.TicketDescription,              
            //    Status = supportTicket.Status,

            //};

            //var Ticket = new SupportTicket
            //{
                
            //    UserId =  userID,
            //    TicketTitle = supportTicket.TicketTitle,
            //    TicketDescription = supportTicket.TicketDescription,
            //    CreatedAt = DateTime.UtcNow,
            //    Status = supportTicket.Status.ToString(),

            //};

            
        }
    }
}
