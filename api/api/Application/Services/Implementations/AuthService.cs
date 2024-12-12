using api.Application.Dtos;
using api.Application.Services.Interfaces;

namespace api.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _tokenService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserService userService, IJwtTokenService jwtTokenService,
        ICurrentUserService currentUserService, IPasswordHasher passwordHasher)
    {
        _userService = userService;
        _tokenService = jwtTokenService;
        _currentUserService = currentUserService;
        _passwordHasher = passwordHasher;
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
        ValidateLoginInput(loginDto);

        var user = _userService.GetUserByEmail(loginDto.Email) ?? 
                   throw new UnauthorizedAccessException("Invalid email or password.");

        CheckPassword(user.Password, loginDto.Password);

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name
        };

        var token = _tokenService.GenerateToken(userDto);

        return new AuthResponseDto
        {
            Token = token
        };
    }
    
    private void ValidateLoginInput(LoginDto loginDto)
    {
        if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
        {
            throw new ArgumentException("Email and Password are required.");
        }
    }
    
    private void CheckPassword(string storedPassword, string inputPassword)
    {
        if (!_passwordHasher.CheckPassword(storedPassword, inputPassword))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }
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