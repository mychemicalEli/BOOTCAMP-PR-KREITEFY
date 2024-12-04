using api.Application.Dtos;

namespace api.Application.Services.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(UserDto user);
}