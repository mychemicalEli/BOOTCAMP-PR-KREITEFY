using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Infrastructure.Persistence;

namespace api.Infrastructure.Persistence;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly KreitekfyContext _context;

    public RoleRepository(KreitekfyContext context) : base(context)
    {
        _context = context;
    }
    public bool Exists(long roleId)
    {
        return _context.Roles.Any(r => r.Id == roleId);
    }
}