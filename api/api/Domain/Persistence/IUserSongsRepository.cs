using api.Application.Dtos;
using api.Domain.Entities;
using framework.Application;
using framework.Domain.Persistence;

namespace api.Domain.Persistence;

public interface IUserSongsRepository : IGenericRepository<UserSongs>
{
    Task<UserSongs?> GetByUserIdAndSongIdAsync(long userId, long songId);
    Task InsertAsync(UserSongs userSong);
    Task UpdateAsync(UserSongs userSong);
    Task<IEnumerable<SongsForYouDto>> GetSongsForYouAsync(long userId);
    Task<PagedList<HistorySongsDto>> GetHistorySongsAsync(long userId, PaginationParameters paginationParameters);
}