using api.Application.Dtos;
using api.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [Produces("application/json")]
    public ActionResult<AuthResponseDto> Register([FromBody] UserDto userDto)
    {
        try
        {
            var response = _authService.Register(userDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    [Produces("application/json")]
    public ActionResult<AuthResponseDto> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var response = _authService.Login(loginDto);
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid email or password.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}