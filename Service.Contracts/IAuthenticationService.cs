using Microsoft.AspNetCore.Identity;
using Shared.DTOs;

namespace Service.Contracts;
public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(RegisterUserDto userDto);
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(string refreshToken);
    Task LogoutAllAsync(string userId);
}
