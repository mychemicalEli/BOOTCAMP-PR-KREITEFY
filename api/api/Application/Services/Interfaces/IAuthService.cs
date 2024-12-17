using api.Application.Dtos;

namespace api.Application.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(UserDto userDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<UserDtoResponse> GetMe();
}