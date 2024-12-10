using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Infrastructure.Persistence;

namespace api.Infrastructure.Persistence;

public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
{
    public AlbumRepository(KreitekfyContext context) : base(context)
    {
    }
}