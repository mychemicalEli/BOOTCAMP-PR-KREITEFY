using api.Application.Dtos;
using framework.Application;
using framework.Application.Services;

namespace api.Application.Services.Interfaces;

public interface IUserSongsService: IGenericService<UserSongsDto>
{
    Task IncrementStreamsAsync(long userId, long songId);
    Task<IEnumerable<SongsForYouDto>> GetSongsForYouAsync(long userId);
    Task<PagedList<HistorySongsDto>> GetHistorySongsAsync(long userId, PaginationParameters paginationParameters);
}