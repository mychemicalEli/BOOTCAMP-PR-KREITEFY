using api.Application.Dtos;
using framework.Application;
using framework.Application.Services;

namespace api.Application.Services.Interfaces;

public interface ISongService : IGenericService<SongDto>
{
    Task<SongDto> GetSongByIdAsync(long id);
    Task<SongDto> InsertAsync(SongDto songDto);
    Task<SongDto> UpdateAsync(long id, SongDto songDto);
    Task<PagedList<SongDto>> GetSongsByCriteriaPagedAsync(string? filter, PaginationParameters paginationParameters);
    Task<IEnumerable<LatestSongsResponse>> GetLatestSongsAsync(int count = 5, long? genreId = null);
    Task<IEnumerable<MostPlayedSongsDto>> GetMostPlayedSongsAsync(int count = 5, long? genreId = null);
}