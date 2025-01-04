using api.Application.Dtos;
using api.Domain.Entities;
using framework.Domain.Persistence;

namespace api.Domain.Persistence;

public interface IUserRepository : IGenericRepository<User>
{
    Task<string> HandlePasswordUpdateAsync(long userId);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User> InsertAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<User> GetByIdAsync(long id);
    Task<List<UserDto>> GetAllUsersWithRoleNameAsync();
}