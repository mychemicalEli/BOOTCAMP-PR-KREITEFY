using api.Application.Dtos;
using api.Domain.Entities;
using framework.Application;
using framework.Domain.Persistence;

namespace api.Domain.Persistence;

public interface ISongRepository : IGenericRepository<Song>
{
    PagedList<SongDto> GetSongsByCriteriaPaged(string? filter, PaginationParameters paginationParameters);
}