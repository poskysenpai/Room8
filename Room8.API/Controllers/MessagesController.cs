using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Room8.Core.Abstractions;
using Room8.Domain.Entities;
using Room8.Infrastructure.Abstractions;

namespace Room8.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] long chatRoomId)
        {
            var response = await _messageService.GetMessages(chatRoomId);
            return Ok(response);
        }
    }
}
