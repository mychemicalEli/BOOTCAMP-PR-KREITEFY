using api.Application.Dtos;

namespace api.Domain.Persistence;

public interface IJwtTokenService
{
    string GenerateToken(UserDto user);
}