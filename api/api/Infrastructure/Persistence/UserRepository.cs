using api.Application.Dtos;
using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Persistence;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private KreitekfyContext _context;

    public UserRepository(KreitekfyContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<List<UserDto>> GetAllUsersWithRoleNameAsync()
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                LastName = u.LastName,
                Email = u.Email,
                Password = u.Password,
                RoleId = u.RoleId,
                RoleName = u.Role.Name
            })
            .ToListAsync();
    }
    
    public async Task<User?> GetByIdAsync(long id)
    {
        return await _context.Users
            .Include(i => i.Role) 
            .SingleOrDefaultAsync(i => i.Id == id); 
    }
    

    public async Task<User> InsertAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        await _context.Entry(user).Reference(i => i.Role).LoadAsync();
        return user;
    }
    
    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        await _context.Entry(user).Reference(i => i.Role).LoadAsync();
        return user;
    }
    
    public async Task<string> HandlePasswordUpdateAsync(long userId)
    {
        var existingUserPassword = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => u.Password)
            .FirstOrDefaultAsync();

        return existingUserPassword ?? throw new Exception("Existing user not found or missing password.");
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Where(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
            .Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                LastName = u.LastName,
                Email = u.Email,
                Password = u.Password,
                RoleId = u.RoleId
            })
            .FirstOrDefaultAsync();
    }
}