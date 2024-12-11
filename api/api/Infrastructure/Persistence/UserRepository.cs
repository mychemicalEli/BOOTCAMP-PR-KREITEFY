using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Domain.Persistence;
using framework.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Persistence;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private KreitekfyContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UserRepository(KreitekfyContext context, IPasswordHasher passwordHasher) : base(context)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public List<UserDto> GetAllUsersWithRoleName()
    {
        return _context.Users
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
            .ToList();
    }


    public override User GetById(long id)
    {
        var user = _context.Users.Include(i => i.Role).SingleOrDefault(i => i.Id == id);
        if (user == null)
        {
            throw new ElementNotFoundException();
        }

        return user;
    }

    public override User Insert(User user)
    {
        user.Password = _passwordHasher.HashPassword(user.Password);
        _context.Users.Add(user);
        _context.SaveChanges();
        _context.Entry(user).Reference(i => i.Role).Load();
        return user;
    }

    public override User Update(User user)
    {
        user.Password = HandlePasswordUpdate(user);

        _context.Users.Update(user);
        _context.SaveChanges();
        _context.Entry(user).Reference(i => i.Role).Load();

        return user;
    }

    private string HandlePasswordUpdate(User user)
    {
        if (!string.IsNullOrEmpty(user.Password))
        {
            return _passwordHasher.HashPassword(user.Password);
        }

        var existingUserPassword = _context.Users
            .AsNoTracking()
            .Where(u => u.Id == user.Id)
            .Select(u => u.Password)
            .FirstOrDefault();

        return existingUserPassword ?? throw new Exception("Existing user not found or missing password.");
    }


    public User? GetUserByEmail(string email)
    {
        return _context.Users
            .Where(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
            .Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                LastName = u.LastName,
                Email = u.Email,
                Password = u.Password,
                RoleId = u.RoleId,
            })
            .FirstOrDefault();
    }
}