using api.Application.Dtos;
using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Application;
using framework.Infrastructure.Persistence;
using framework.Infrastructure.Specs;

namespace api.Infrastructure.Persistence;

public class UserSongsRepository : GenericRepository<UserSongs>, IUserSongsRepository
{
    private readonly KreitekfyContext _context;
    private readonly ISpecificationParser<UserSongs> _specificationParser;

    public UserSongsRepository(KreitekfyContext context, ISpecificationParser<UserSongs> specificationParser) : base(context)
    {
        _context = context;
        _specificationParser = specificationParser;
    }

    public PagedList<HistorySongsDto> GetHistorySongs(long userId, PaginationParameters paginationParameters)
    {
        var userSongsHistory = _context.UserSongs
            .Where(us => us.UserId == userId) 
            .OrderByDescending(us => us.TotalStreams) 
            .AsQueryable();
        
        if (string.IsNullOrEmpty(paginationParameters.Sort))
        {
            userSongsHistory = userSongsHistory.OrderByDescending(us => us.LastPlayedAt);
        }
        
        var userSongsHistoryDto = userSongsHistory.Select(i => new HistorySongsDto()
        {
            SongId = i.Song.Id,
            Title = i.Song.Title,
            LastPlayedAt = i.LastPlayedAt.Value,
        });
        
        return PagedList<HistorySongsDto>.ToPagedList(
            userSongsHistoryDto,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );
    }


    public UserSongs? GetByUserIdAndSongId(long userId, long songId)
    {
        return _context.Set<UserSongs>()
            .SingleOrDefault(us => us.UserId == userId && us.SongId == songId);
    }
    
    public IEnumerable<SongsForYouDto> GetSongsForYou(long userId)
    {
        // 1. Obtener los géneros más escuchados por el usuario
        var topGenres = _context.UserSongs
            .Where(us => us.UserId == userId)
            .GroupBy(us => us.Song.Genre.Name)
            .Select(g => new
            {
                GenreName = g.Key,
                TotalStreams = g.Sum(us => us.TotalStreams) 
            })
            .OrderByDescending(g => g.TotalStreams)
            .Take(2) 
            .Select(g => g.GenreName)
            .ToList();

        // 2. Filtrar canciones por las más escuchadas con calificación >= 3
        var filteredSongs = _context.UserSongs
            .Where(us => us.UserId == userId && topGenres.Contains(us.Song.Genre.Name) && us.Song.MediaRating >= 3)
            .OrderByDescending(us => us.Song.Streams)
            .Take(5) 
            .Select(us => new SongsForYouDto
            {
                Id = us.Song.Id,
                Title = us.Song.Title,
                ArtistName = us.Song.Artist.Name,
                AlbumCover = Convert.ToBase64String(us.Song.Album.Cover),
                GenreName = us.Song.Genre.Name,
                Streams = us.Song.Streams,
                MediaRating = us.Song.MediaRating
            })
            .ToList();

        return filteredSongs;
    }
}