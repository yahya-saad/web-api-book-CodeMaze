using Domain.Entities;

namespace Contracts;
public interface ITokenProvider
{
    Task<string> GenerateAccessTokenAsync(User user);
    Task<string> GenerateRefreshTokenAsync();
}
