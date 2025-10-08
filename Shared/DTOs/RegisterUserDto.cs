using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;
public class RegisterUserDto
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; init; } = default!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; init; } = default!;

    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string? Email { get; init; }

    public string? PhoneNumber { get; init; }

    public ICollection<string>? Roles { get; init; }
}
