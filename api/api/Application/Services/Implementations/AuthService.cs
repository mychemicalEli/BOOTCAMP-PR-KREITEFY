using api.Application.Dtos;
using api.Application.Services.Interfaces;

namespace api.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _tokenService;
    private readonly ICurrentUserService _currentUserService;

    public AuthService(IUserService userService, IJwtTokenService jwtTokenService, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _tokenService = jwtTokenService;
        _currentUserService = currentUserService;
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
        if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
        {
            throw new ArgumentException("Email and Password are required.");
        }
        
        var user = _userService.GetUserByEmail(loginDto.Email);
        
        if (user == null || user.Password != loginDto.Password)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }
        
        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name, 
        };
        
        var token = _tokenService.GenerateToken(userDto);

        return new AuthResponseDto
        {
            Token = token
        };
    }
    
    public UserDtoResponse GetMe()
    {
        var user = _currentUserService.GetUserFromToken();
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid or expired token.");
        }
        return user;
    }
}