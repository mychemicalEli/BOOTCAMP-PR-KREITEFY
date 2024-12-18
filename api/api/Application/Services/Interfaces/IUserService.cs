using api.Application.Dtos;
using framework.Application.Services;

namespace api.Application.Services.Interfaces;

public interface IUserService : IGenericService<UserDto>
{
    List<UserDto> GetAllUsersWithRoleName();
    UserDto RegisterUser(UserDto userDto);
    UserDto GetUserByEmail(string email);
}