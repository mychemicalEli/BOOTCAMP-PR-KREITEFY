using api.Application.Dtos;

namespace api.Application.Services.Interfaces;

public interface IAuthService
{
    AuthResponseDto Register(UserDto userDto);
    AuthResponseDto Login(LoginDto loginDto);
    string GenerateJwtToken(UserDto user);
}