using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Domain.Persistence;
using framework.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Persistence;

public class UserSongsRepository: GenericRepository<UserSongs>, IUserSongsRepository
{
    private readonly KreitekfyContext _context;

    public UserSongsRepository(KreitekfyContext context) : base(context)
    {
        _context = context;
    }
    
    public UserSongs? GetByUserIdAndSongId(long userId, long songId)
    {
        return _context.Set<UserSongs>().FirstOrDefault(us => us.UserId == userId && us.SongId == songId);
    }
    
    public override UserSongs GetById(long id)
    {
        var userSong = _context.UserSongsHistories
            .Include(us => us.User)
            .Include(us => us.Song)
            .SingleOrDefault(us => us.Id == id);

        if (userSong == null)
        {
            throw new ElementNotFoundException();
        }

        return userSong;
    }
    
}