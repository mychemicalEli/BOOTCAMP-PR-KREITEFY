using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using api.Application.Dtos;
using api.Domain.Persistence;
using Microsoft.IdentityModel.Tokens;
using JwtSecurityToken = System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

namespace api.Infrastructure.Persistence;

public class JwtTokenService: IJwtTokenService
{
    public string GenerateToken(UserDto user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, Uri.EscapeDataString(user.Name)),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(RandomNumberGenerator.GetBytes(32));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}