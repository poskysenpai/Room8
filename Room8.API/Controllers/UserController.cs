using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Room8.API.Dtos;
using Room8.Core.Abstractions;
using Room8.Core.Dtos;
using Room8.Core.Implementations;
using Room8.Data.Context;
using Room8.Domain.Entities;

namespace Room8.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        public UserController(IUserService userService, UserManager<User> userManager, AppDbContext appDbContext)
        {
            _userService = userService;
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpPut("EditProfile")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileDto editProfileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userService.EditProfile(editProfileDto);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var response = await _userService.GetUserById(id);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _userService.GetUsers(pageNumber, pageSize);
            return Ok(response);
        }
    }

}
