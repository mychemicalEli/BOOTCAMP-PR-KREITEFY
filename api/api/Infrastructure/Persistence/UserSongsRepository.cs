using api.Application.Dtos;
using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Application;
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

    public async Task<PagedList<HistorySongsDto>> GetHistorySongsAsync(long userId, PaginationParameters paginationParameters)
    {
        var userSongsHistory = _context.UserSongs
            .Where(us => us.UserId == userId)
            .OrderByDescending(us => us.LastPlayedAt)
            .Select(i => new HistorySongsDto
            {
                SongId = i.Song.Id,
                Title = i.Song.Title,
                Artist = i.Song.Artist.Name,
                LastPlayedAt = i.LastPlayedAt.Value,
            });

        return PagedList<HistorySongsDto>.ToPagedList(
            userSongsHistory,
            paginationParameters.PageNumber,
            paginationParameters.PageSize
        );
    }

    public async Task<UserSongs?> GetByUserIdAndSongIdAsync(long userId, long songId)
    {
        return await _context.Set<UserSongs>()
            .SingleOrDefaultAsync(us => us.UserId == userId && us.SongId == songId);
    }

    public async Task<IEnumerable<SongsForYouDto>> GetSongsForYouAsync(long userId)
    {
        var topGenres = await _context.UserSongs
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
            .ToListAsync();

        var filteredSongs = await _context.UserSongs
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
            .ToListAsync();

        return filteredSongs;
    }
    
    public async Task InsertAsync(UserSongs userSong)
    {
        if (userSong == null)
        {
            throw new ArgumentNullException(nameof(userSong));
        }
        await _context.Set<UserSongs>().AddAsync(userSong);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(UserSongs userSong)
    {
        if (userSong == null)
        {
            throw new ArgumentNullException(nameof(userSong));
        }
        var existingUserSong = await _context.Set<UserSongs>()
            .SingleOrDefaultAsync(us => us.Id == userSong.Id);

        if (existingUserSong == null)
        {
            throw new InvalidOperationException("UserSong not found");
        }
        _context.Entry(existingUserSong).CurrentValues.SetValues(userSong);
        await _context.SaveChangesAsync();
    }

}