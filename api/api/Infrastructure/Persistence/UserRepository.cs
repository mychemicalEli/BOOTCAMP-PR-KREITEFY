using api.Application.Dtos;
using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Domain.Persistence;
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
        _context.Users.Add(user);
        _context.SaveChanges();
        _context.Entry(user).Reference(i => i.Role).Load();
        return user;
    }

    public override User Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
        _context.Entry(user).Reference(i => i.Role).Load();
        return user;
    }
}