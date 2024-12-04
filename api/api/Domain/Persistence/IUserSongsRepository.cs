using api.Domain.Entities;
using framework.Domain.Persistence;

namespace api.Domain.Persistence;

public interface IUserSongsRepository : IGenericRepository<UserSongs>
{
    UserSongs? GetByUserIdAndSongId(long userId, long songId);
}