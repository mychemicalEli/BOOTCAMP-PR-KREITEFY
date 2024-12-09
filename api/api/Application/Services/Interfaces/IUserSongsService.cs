using api.Application.Dtos;
using framework.Application;
using framework.Application.Services;

namespace api.Application.Services.Interfaces;

public interface IUserSongsService: IGenericService<UserSongsDto>
{
    void IncrementStreams(long userId, long songId);
    IEnumerable<SongsForYouDto> GetSongsForYou(long userId);
    PagedList<HistorySongsDto> GetHistorySongs(long userId, PaginationParameters paginationParameters);
}