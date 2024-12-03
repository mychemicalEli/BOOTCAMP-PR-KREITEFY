using api.Application.Dtos;

namespace api.Domain.Persistence;

public interface IJwtToken
{
    string GenerateToken(UserDto user);
    string ExtractUserNameFromToken(string token);
}