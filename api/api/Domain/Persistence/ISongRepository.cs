using api.Application.Dtos;
using api.Domain.Entities;
using framework.Application;
using framework.Domain.Persistence;

namespace api.Domain.Persistence;

public interface ISongRepository : IGenericRepository<Song>
{
    Task<Song> GetByIdAsync(long id);
    Task<Song> UpdateAsync(Song song);
    Task<Song> InsertAsync(Song song);
    Task<PagedList<SongDto>> GetSongsByCriteriaPagedAsync(string? filter, PaginationParameters paginationParameters);
    Task<IEnumerable<LatestSongsResponse>> GetLatestSongsAsync(int count = 5, long? genreId = null);
    Task<IEnumerable<MostPlayedSongsDto>> GetMostPlayedSongsAsync(int count = 5, long? genreId = null);
}