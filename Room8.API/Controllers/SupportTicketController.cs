using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Room8.API.Services;
using Room8.Core.Abstractions;
using Room8.Core.Dtos;
using Room8.Core.Implementations;
using Room8.Domain.Entities;

namespace Room8.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

   

    public class SupportTicketController : ControllerBase
    {
        private readonly ISupportTicketService _supportTicketService;
        private readonly UserManager<User> _userManager;
        public SupportTicketController(ISupportTicketService supportTicketService, UserManager<User> userManager)
        {
            _supportTicketService = supportTicketService;
            _userManager = userManager;
        }

       // [Authorize(Roles = "Admin")]
        [HttpGet("all-support-tickets")]
        public async Task<IActionResult> GetAllSupportTickets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            var supportTickets = await _supportTicketService.GetAllSupportTickets(pageNumber, pageSize);
            if (supportTickets != null)
            {
                return Ok(supportTickets);
            }

            return StatusCode(supportTickets.StatusCode, supportTickets);

        }

        //[HttpPost("Create")]
        //public async Task<IActionResult> Create(SupportTicketDTO supportTicket)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var response = await _supportTicketService.CreateSupportTicket(supportTicket, user);

        //    if (response != null)
        //    {
        //        return Ok(response);

        //    }
        //    return StatusCode(response.StatusCode, response);


        //}
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SupportTicketDTO supportTicket)
        {
            
            var response = await _supportTicketService.CreateSupportTicket(supportTicket);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("support-ticket/{id}")]

        public async Task<IActionResult> GetSupportTicket(long id)
        {
            var supportTicket = await _supportTicketService.GetSupportTicket(id);

            if (supportTicket.IsSuccessful)
            {
                return Ok(supportTicket);
            }
            return StatusCode(supportTicket.StatusCode, supportTicket);
        }

        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(long ticketId)
        {
            var response = await _supportTicketService.UpdateStatus(ticketId);

            if (!response.IsSuccessful)
            {
                return StatusCode(400, "Invalide request");
            }

            return Ok(response.Data);
        }
    }


    
}
