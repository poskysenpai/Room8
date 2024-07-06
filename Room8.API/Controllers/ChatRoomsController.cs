using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Room8.Core.Abstractions;

namespace Room8.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomsController : ControllerBase
    {
        private readonly IChatRoomService _chatRoomService;
        public ChatRoomsController(IChatRoomService chatRoomService) 
        {
            _chatRoomService = chatRoomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetChatRooms([FromQuery] string userId)
        {
            var response = await _chatRoomService.GetChatRooms(userId);
            return Ok(response);
        }
    }
}
