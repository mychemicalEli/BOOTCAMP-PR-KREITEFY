using api.Application.Dtos;
using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Infrastructure.Persistence;

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
        return _context.Set<UserSongs>()
            .SingleOrDefault(us => us.UserId == userId && us.SongId == songId);
    }
    
    public IEnumerable<SongsForYouDto> GetSongsForYou(long userId)
    {
        var userSongs = _context.Set<UserSongs>()
            .Where(us => us.UserId == userId)
            .Select(us => new
            {
                us.Song.Id,
                us.Song.Title,
                us.Song.Artist.Name,
                GenreName = us.Song.Genre.Name,
                us.Song.Streams,
                us.Song.MediaRating,
                us.TotalStreams,
                AlbumCover = Convert.ToBase64String(us.Song.Album.Cover)
            })
            .ToList();

        // Obtener los 2 géneros más escuchados
        var topGenres = userSongs
            .GroupBy(us => us.GenreName)
            .Select(g => new
            {
                g.Key,
                TotalStreams = g.Sum(us => us.TotalStreams)
            })
            .OrderByDescending(g => g.TotalStreams)
            .Take(2)
            .Select(g => g.Key)
            .ToList();

        // Filtrar y agrupar canciones por género y calificación media
        return userSongs
            .Where(us => topGenres.Contains(us.GenreName) && us.MediaRating >= 3)
            .GroupBy(us => us.GenreName)
            .Select(g => new SongsForYouDto
            {
                GenreName = g.Key,
                TopSongs = g
                    .OrderByDescending(us => us.Streams)
                    .Take(5)
                    .Select(us => new UserSelectedSongsDto
                    {
                        Id = us.Id,
                        Title = us.Title,
                        ArtistName = us.Name,
                        AlbumCover = us.AlbumCover,
                        GenreName = us.GenreName,
                        Streams = us.Streams,
                        MediaRating = us.MediaRating
                    })
                    .ToList()
            })
            .ToList();
    }

}