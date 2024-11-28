using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Infrastructure.Persistence;

namespace api.Infrastructure.Persistence;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(KreitekfyContext context) : base(context)
    {
    }
}