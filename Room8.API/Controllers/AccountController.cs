using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Room8.Core.Abstractions;
using Room8.Core.Dtos;
using Room8.Domain.Entities;

namespace Room8.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;

        public AccountController(IAuthService authService, UserManager<User> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            var result = await _authService.Register(registrationRequestDTO);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            var result = await _authService.ForgotPasswordAsync(email);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
			var result = await _authService.ConfirmEmail(email, token);
			if (result.IsSuccessful)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var response = await _authService.ResetPassword(resetPasswordDto);
			if (response.IsSuccessful)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var loginResult = await _authService.Login(model);
            if(loginResult.IsSuccessful)
            {
                return Ok(loginResult);
            }
            return BadRequest(loginResult);
        }

    }
}

