using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Persistence;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly KreitekfyContext _context;

    public RoleRepository(KreitekfyContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(long roleId)
    {
        return await _context.Roles
            .AnyAsync(r => r.Id == roleId);
    }
}