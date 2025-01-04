using api.Application;
using api.Application.Dtos;
using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Application;
using framework.Infrastructure.Persistence;
using framework.Infrastructure.Specs;
using framework.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Persistence
{
    public class SongRepository : GenericRepository<Song>, ISongRepository
    {
        private readonly KreitekfyContext _context;
        private readonly ISpecificationParser<Song> _specificationParser;

        public SongRepository(KreitekfyContext context, ISpecificationParser<Song> specificationParser) : base(context)
        {
            _context = context;
            _specificationParser = specificationParser;
        }

        public async Task<Song> GetByIdAsync(long id)
        {
            var song = await _context.Songs
                .Include(i => i.Album)
                .Include(i => i.Artist)
                .Include(i => i.Genre)
                .SingleOrDefaultAsync(i => i.Id == id);

            if (song == null)
            {
                throw new ElementNotFoundException();
            }

            return song;
        }

        public async Task<PagedList<SongDto>> GetSongsByCriteriaPagedAsync(string? filter,
            PaginationParameters paginationParameters)
        {
            var songs = _context.Songs.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                Specification<Song> specification = _specificationParser.ParseSpecification(filter);
                songs = specification.ApplySpecification(songs);
            }

            if (!string.IsNullOrEmpty(paginationParameters.Sort))
            {
                songs = ApplySortOrder(songs, paginationParameters.Sort);
            }

            var songsDto = songs.Select(i => new SongDto()
            {
                Id = i.Id,
                Title = i.Title,
                AlbumId = i.AlbumId,
                AlbumName = i.Album.Title,
                AlbumCover = Convert.ToBase64String(i.Album.Cover),
                ArtistId = i.ArtistId,
                ArtistName = i.Artist.Name,
                GenreId = i.GenreId,
                GenreName = i.Genre.Name,
                Duration = TimeSpanConverter.ToString(i.Duration),
                Streams = i.Streams,
                MediaRating = i.MediaRating,
                AddedAt = i.AddedAt
            });

            var pagedSongs = PagedList<SongDto>.ToPagedList(songsDto, paginationParameters.PageNumber,
                paginationParameters.PageSize);
            return pagedSongs;
        }


        public async Task<Song> InsertAsync(Song song)
        {
            await _context.Songs.AddAsync(song);
            await _context.SaveChangesAsync();
            await _context.Entry(song).Reference(i => i.Genre).LoadAsync();
            await _context.Entry(song).Reference(i => i.Album).LoadAsync();
            await _context.Entry(song).Reference(i => i.Artist).LoadAsync();
            return song;
        }


        public async Task<Song> UpdateAsync(Song song)
        {
            if (song == null)
            {
                throw new ArgumentNullException(nameof(song));
            }
            _context.Songs.Update(song);
            await _context.SaveChangesAsync();
            await _context.Entry(song).Reference(i => i.Genre).LoadAsync();
            await _context.Entry(song).Reference(i => i.Album).LoadAsync();
            await _context.Entry(song).Reference(i => i.Artist).LoadAsync();
            return song;
        }
        
        public async Task<IEnumerable<LatestSongsResponse>> GetLatestSongsAsync(int count = 5, long? genreId = null)
        {
            var songs = await _context.Songs
                .Where(song => !genreId.HasValue || song.GenreId == genreId)
                .OrderByDescending(song => song.AddedAt)
                .Take(count)
                .Select(i => new LatestSongsResponse
                {
                    Id = i.Id,
                    Title = i.Title,
                    AlbumCover = Convert.ToBase64String(i.Album.Cover),
                    ArtistName = i.Artist.Name,
                    AddedAt = i.AddedAt
                })
                .ToListAsync();

            return songs;
        }

        public async Task<IEnumerable<MostPlayedSongsDto>> GetMostPlayedSongsAsync(int count = 5, long? genreId = null)
        {
            var songs = await _context.Songs
                .Where(song => !genreId.HasValue || song.GenreId == genreId)
                .OrderByDescending(song => song.Streams)
                .Take(count)
                .Select(i => new MostPlayedSongsDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    AlbumCover = Convert.ToBase64String(i.Album.Cover),
                    ArtistName = i.Artist.Name,
                    Streams = i.Streams
                })
                .ToListAsync();

            return songs;
        }
    }
}