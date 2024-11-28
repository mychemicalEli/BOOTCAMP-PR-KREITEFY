using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Infrastructure.Persistence;

namespace api.Infrastructure.Persistence;

public class ArtistRepository : GenericRepository<Artist>, IArtistRepository
{
    public ArtistRepository(KreitekfyContext context) : base(context)
    {
    }
}