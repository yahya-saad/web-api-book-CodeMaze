using AutoMapper;
using Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Shared.DTOs;

namespace Service.Services;
internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenProvider tokenProvider;

    public AuthenticationService(ILoggerManager logger, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ITokenProvider tokenProvider)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
        this.tokenProvider = tokenProvider;
    }

    public async Task<IdentityResult> RegisterUser(RegisterUserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (!result.Succeeded)
            return result;

        if (userDto.Roles is not null && userDto.Roles.Any())
        {
            var validRoles = await _roleManager.Roles
                .Select(r => r.Name)
                .ToListAsync();

            var invalidRoles = userDto.Roles
                .Where(role => !validRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
                .ToList();

            if (invalidRoles.Any())
            {
                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "InvalidRole",
                        Description = $"The following roles do not exist: {string.Join(", ", invalidRoles)}"
                    }
                );
            }

            await _userManager.AddToRolesAsync(user, userDto.Roles);
        }

        return result;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);

        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return null;

        var accessToken = await tokenProvider.GenerateAccessTokenAsync(user);
        var refreshToken = await tokenProvider.GenerateRefreshTokenAsync();

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
        });

        await _userManager.UpdateAsync(user);

        return new AuthResponseDto(accessToken, refreshToken);
    }

    public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
    {
        var user = _userManager.Users
        .Include(u => u.RefreshTokens)
        .FirstOrDefault(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

        if (user == null) return null;

        var storedToken = user.RefreshTokens.Single(x => x.Token == refreshToken);

        if (!storedToken.IsActive) return null;

        // mark old token as used/revoked
        storedToken.IsUsed = true;
        storedToken.RevokedAt = DateTime.UtcNow;

        var newAccessToken = await tokenProvider.GenerateAccessTokenAsync(user);
        var newRefreshToken = await tokenProvider.GenerateRefreshTokenAsync();

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
        });


        await _userManager.UpdateAsync(user);
        return new AuthResponseDto(newAccessToken, newRefreshToken);
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var user = _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefault(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

        if (user == null) return;

        var token = user.RefreshTokens.Single(t => t.Token == refreshToken);
        token.RevokedAt = DateTime.UtcNow;
        token.IsUsed = true;

        await _userManager.UpdateAsync(user);
    }

    public async Task LogoutAllAsync(string userId)
    {
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return;

        foreach (var token in user.RefreshTokens.Where(t => t.IsActive))
        {
            token.RevokedAt = DateTime.UtcNow;
            token.IsUsed = true;
        }

        await _userManager.UpdateAsync(user);
    }
}