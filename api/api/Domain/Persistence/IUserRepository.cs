using api.Application.Dtos;
using api.Domain.Entities;
using framework.Domain.Persistence;

namespace api.Domain.Persistence;

public interface IUserRepository : IGenericRepository<User>
{
    List<UserDto> GetAllUsersWithRoleName();
}