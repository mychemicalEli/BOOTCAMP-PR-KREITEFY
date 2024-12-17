using api.Application.Dtos;
using framework.Application.Services;

namespace api.Application.Services.Interfaces;

public interface IUserService : IGenericService<UserDto>
{
    Task<List<UserDto>> GetAllUsersWithRoleNameAsync();
    Task<UserDto> RegisterUserAsync(UserDto userDto);
    Task<UserDto> GetUserByEmailAsync(string email);
    Task<UserDto> InsertAsync(UserDto userDto);
    Task<UserDto> UpdateAsync(UserDto userDto);
    Task<UserDto> GetByIdAsync(long id);
}