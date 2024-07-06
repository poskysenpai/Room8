
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Room8.Core.Dtos;
using System.Security.Claims;
using Room8.Core.Abstractions;
using Room8.Data.Context;
using Room8.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Room8.Core.Implementations
{
    public class AuthService : IAuthService
    {

        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private IConfiguration _config;
        public AuthService(AppDbContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor,
            IEmailService emailService, ITokenService tokenService, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _tokenService = tokenService;
            _config = configuration;
        }

        public async Task<ResponseDto<UserDto>> ForgotPasswordAsync(string email)
        {
            var errors = new List<Error>();

            // Verify User with this email exists
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                //throw new ResourceNotFoundException("User with this email does not exist.");
                errors.Add(new Error("400", "User with this email does not exist."));
                return ResponseDto<UserDto>.Failure(errors, 400);
            }

            // Generate the password reset token
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var url = _config.GetSection("FrontEndUrl").Value;

            if(string.IsNullOrEmpty(url))
            {
                errors.Add(new Error("400", "The frontend url is missing in the app settings"));
                return ResponseDto<UserDto>.Failure(errors, 400);
            }
            // Create the email body
            string emailBody = CreateForgotPasswordEmailBody(user, token, url);

            // Send the email
            var responseMsg = await _emailService.SendEmail(email, "Reset Password Request", emailBody);

            if (responseMsg != string.Empty)
            {
                //throw new InternalServerException(responseMsg);
                errors.Add(new Error("500", responseMsg));
                return ResponseDto<UserDto>.Failure(errors, 500);
            }

            return ResponseDto<UserDto>.Success("Successfully sent reset token to User's email.", 200);
        }

        private static string CreateForgotPasswordEmailBody(User user, string token, string url)
        {
            string resetUrl = $"{url}/reset-password?email={user.Email}&token={Uri.EscapeDataString(token)}";
            return $@"
                        <html>
                        <body>
                            <p>Dear {user.UserName},</p>
                            <p>You requested to reset your password. Please click the link below to reset your password:</p>
                            <p><a href='{resetUrl}'>Reset Password</a></p>
                            <p>If you did not request a password reset, please ignore this email.</p>
                            <p>Thank you,</p>
                            <p>Room8s Team</p>
                        </body>
                        </html>";
        }

        public async Task<ResponseDto<UserDto>> Register(RegistrationRequestDTO registrationRequestDTO)
        {

            var user = await _userManager.FindByEmailAsync(registrationRequestDTO.Email);

            if (user != null )
            {
                var errors = new List<Error>
                  {
                   new Error("400", "User already exists.")
                      };
                return ResponseDto<UserDto>.Failure(errors, 400);
            }


            User userToAdd = new()
            {
                UserName = registrationRequestDTO.Email,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                FirstName = registrationRequestDTO.Firstname,
                LastName = registrationRequestDTO.Lastname,
            };


            var result = await _userManager.CreateAsync(userToAdd, registrationRequestDTO.Password);


            if (!result.Succeeded)
                return ResponseDto<UserDto>.Failure(result.Errors.Select(e => new Error(e.Code, e.Description)), 500);

            var userToReturn = _userManager.Users.First(u => u.UserName == registrationRequestDTO.Email);


            if (userToReturn != null)
            {

                var requestScheme = _httpContextAccessor.HttpContext.Request.Scheme;
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(userToAdd);
                var confirmationLink = $"{requestScheme}://localhost:7228/api/Account/confirm-email?email={Uri.EscapeDataString(userToAdd.Email)}&token={Uri.EscapeDataString(token)}";

                var body = @$"
                  <h1>Confirmation Email</h1>
                   <p>
                    Please confirm your email address by clicking the link below:
                  <a href='{confirmationLink}'>Confirm Email</a>
                      </p>
                        ";

                var response = await _emailService.SendEmail(userToAdd.Email, "Confirmation Link", body);

            }

            await _userManager.AddToRoleAsync(userToAdd, "User");
            //await AssignRole(userToReturn.Email, "USER");
            //var roles = await _userManager.GetRolesAsync(userToReturn);

            UserDto UserDTO = new()
            {

                Email = userToReturn.Email,
                FirstName = userToReturn.FirstName,
                LastName = userToReturn.LastName,
                PhoneNumber = userToReturn.PhoneNumber,
            };

            return ResponseDto<UserDto>.Success(UserDTO, "Successfully Registered New User", 201);
        }

		public async Task<ResponseDto<UserDto>> ConfirmEmail(string email, string token)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user != null)
			{
				var result = await _userManager.ConfirmEmailAsync(user, token);
				if (result.Succeeded)
					return ResponseDto<UserDto>.Success("Email confirmed successfully", 200);
				return ResponseDto<UserDto>.Failure(result.Errors.Select(e => new Error(e.Code, e.Description)), 400);
			}

			var errors = new List<Error>
				  {
				   new Error("400", "User does not exist")
					  };
			return ResponseDto<UserDto>.Failure(errors, 400);


		}

		public async Task<ResponseDto<UserDto>> ResetPassword(ResetPasswordDto ResetPasswordDto)
		{
			var user = await _userManager.FindByEmailAsync(ResetPasswordDto.Email);
			if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, ResetPasswordDto.Token, ResetPasswordDto.NewPassword);

                if (!result.Succeeded)
                    return ResponseDto<UserDto>.Failure(result.Errors.Select(e => new Error(e.Code, e.Description)));
                else
                    return ResponseDto<UserDto>.Success("Successful Password Reset", 200);
            }
            else
            {
                var errors = new List<Error>
                {
                   new Error("404", "User does not exist")
                };
                return ResponseDto<UserDto>.Failure(errors, 404);

            }
        }



        public async Task<ResponseDto<LoginResultDto>> Login(LoginDto loginDTO)
        {
            var errors = new List<Error>();
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, loginDTO.Password))
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    var returnUser = new LoginResultDto()
                    {
                        UserId = user.Id,
                        Roles = roles.ToList(),
                        Token = _tokenService.GenerateJwtToken(user, roles[0])
                    };

                    return ResponseDto<LoginResultDto>.Success(returnUser);

                }
            }

            errors.Add(new Error("400", "Invalid credential"));
            return ResponseDto<LoginResultDto>.Failure(errors, 400);
        }
    }
}

