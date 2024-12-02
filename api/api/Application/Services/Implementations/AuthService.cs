using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using api.Application.Dtos;
using api.Application.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace api.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;

    public AuthService(IUserService userService)
    {
        _userService = userService;
    }

    public AuthResponseDto Register(UserDto userDto)
    {
        var existingUser = _userService.GetUserByEmail(userDto.Email);
        if (existingUser != null)
        {
            throw new Exception("Email is already in use.");
        }

        var newUser = _userService.RegisterUser(userDto);
        var token = GenerateJwtToken(newUser);

        return new AuthResponseDto
        {
            Token = token,
            UserName = newUser.Name
        };
    }

    public AuthResponseDto Login(LoginDto loginDto)
    {
        var user = _userService.GetUserByEmail(loginDto.Email);
        if (user == null || user.Password != loginDto.Password)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            UserName = user.Name
        };
    }

    public string GenerateJwtToken(UserDto user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.Name),
        };

        var key = new SymmetricSecurityKey(RandomNumberGenerator.GetBytes(32));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "your_issuer",
            audience: "your_audience",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}