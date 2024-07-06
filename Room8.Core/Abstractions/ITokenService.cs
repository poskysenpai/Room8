using Microsoft.AspNetCore.Identity;
using Room8.Domain.Entities;

namespace Room8.Core.Abstractions
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user, string role);
    }
}
