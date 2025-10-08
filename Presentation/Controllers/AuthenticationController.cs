using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;

namespace Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(IServiceManager service) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
    {
        var result = await service.AuthenticationService.RegisterUser(userDto);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await service.AuthenticationService.LoginAsync(dto);
        if (result is null)
            return Unauthorized();

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
    {
        var tokens = await service.AuthenticationService.RefreshTokenAsync(dto.RefreshToken);
        if (tokens == null) return Unauthorized();
        return Ok(tokens);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshRequestDto dto)
    {
        await service.AuthenticationService.LogoutAsync(dto.RefreshToken);
        return NoContent();
    }

    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAll([FromQuery] string userId)
    {
        await service.AuthenticationService.LogoutAllAsync(userId);
        return NoContent();
    }

}
