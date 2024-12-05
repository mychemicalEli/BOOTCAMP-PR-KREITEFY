using api.Application.Dtos;
using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Domain.Persistence;
using framework.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Persistence;

public class UserSongsRepository : GenericRepository<UserSongs>, IUserSongsRepository
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
        var userSong = _context.UserSongs
            .Include(us => us.User)
            .Include(us => us.Song)
            .SingleOrDefault(us => us.Id == id);

        if (userSong == null)
        {
            throw new ElementNotFoundException();
        }

        return userSong;
    }

    public IEnumerable<UserSongsDto> GetUserSongsWithSong(long userId)
    {
        return _context.Set<UserSongs>()
            .Where(us => us.UserId == userId)
            .Select(us => new UserSongsDto
            {
                Id = us.UserId,
                UserId = us.UserId,
                SongId = us.SongId,
                LastPlayedAt = us.LastPlayedAt,
                TotalStreams = us.TotalStreams,
                Song = new UserSelectedSongsDto
                {
                    Id = us.Song.Id,
                    Title = us.Song.Title,
                    ArtistName = us.Song.Artist.Name,
                    AlbumCover = Convert.ToBase64String(us.Song.Album.Cover),
                    GenreName = us.Song.Genre.Name,
                    Streams = us.Song.Streams,
                    MediaRating = us.Song.MediaRating
                }
            })
            .ToList();
    }
}