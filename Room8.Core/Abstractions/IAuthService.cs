using Room8.Core.Dtos;

namespace Room8.Core.Abstractions
{
    public interface IAuthService
    {
        Task<ResponseDto<UserDto>> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<ResponseDto<UserDto>> ResetPassword(ResetPasswordDto ResetPasswordDto);
        Task<ResponseDto<UserDto>> ConfirmEmail(string email, string token);
        Task<ResponseDto<UserDto>> ForgotPasswordAsync(string email);
        Task<ResponseDto<LoginResultDto>> Login(LoginDto loginDTO);
    }
}
