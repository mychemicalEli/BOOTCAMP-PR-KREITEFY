using api.Application.Dtos;
using api.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [Produces("application/json")]
    public ActionResult<UserDto> Register([FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existingUser = _userService.GetAllUsersWithRoleName().FirstOrDefault(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                return BadRequest("El correo electrónico ya está registrado.");
            }

            var newUser = _userService.RegisterUser(userDto);
            return newUser;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}