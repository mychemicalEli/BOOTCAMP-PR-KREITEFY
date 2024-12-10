using api.Application.Dtos;
using api.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;

[Route("[controller]")]
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
    [AllowAnonymous]
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
    [AllowAnonymous]
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

    [HttpGet("me")]
    [Produces("application/json")]
    [Authorize]
    public ActionResult<UserDtoResponse> GetMe()
    {
        try
        {
            var user = _authService.GetMe();
            return Ok(user);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}