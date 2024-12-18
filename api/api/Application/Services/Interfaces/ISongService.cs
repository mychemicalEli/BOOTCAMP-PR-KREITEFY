using api.Application.Dtos;
using framework.Application;
using framework.Application.Services;

namespace api.Application.Services.Interfaces;

public interface ISongService : IGenericService<SongDto>
{
    PagedList<SongDto> GetSongsByCriteriaPaged(string? filter, PaginationParameters paginationParameters);
    IEnumerable<LatestSongsResponse> GetLatestSongs(int count = 5, long? genreId = null);
    IEnumerable<MostPlayedSongsDto> GetMostPlayedSongs(int count = 5, long? genreId = null);
}