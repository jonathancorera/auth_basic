using AuthTest.Dtos.Auth;
using AuthTest.Dtos.Users;
using DataAccess.Entities;

namespace AuthTest.Services
{
    public interface IAuthService
    {
        string GenerateTokenString(ApplicationUser user);
        Task<ApplicationUser> Login(LoginRequestDto user);
        Task<bool> RegisterUser(UserCreationDto user);

    }
}