using api.Application.Dtos;
using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Domain.Persistence;
using framework.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Persistence;

public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
{
    public AlbumRepository(KreitekfyContext context) : base(context)
    {
    }
}