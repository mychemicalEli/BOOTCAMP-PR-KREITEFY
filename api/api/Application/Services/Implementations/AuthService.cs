using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Persistence;

namespace api.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _tokenService;

    public AuthService(IUserService userService, IJwtTokenService jwtTokenService)
    {
        _userService = userService;
        _tokenService = jwtTokenService;
    }

    public AuthResponseDto Register(UserDto userDto)
    {
        var existingUser = _userService.GetUserByEmail(userDto.Email);
        if (existingUser != null)
        {
            throw new Exception("Email is already in use.");
        }

        var newUser = _userService.RegisterUser(userDto);
        var token = _tokenService.GenerateToken(newUser);

        return new AuthResponseDto
        {
            Token = token
        };
    }

    public AuthResponseDto Login(LoginDto loginDto)
    {
        var user = _userService.GetUserByEmail(loginDto.Email);
        if (user == null || user.Password != loginDto.Password)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token
        };
    }
}