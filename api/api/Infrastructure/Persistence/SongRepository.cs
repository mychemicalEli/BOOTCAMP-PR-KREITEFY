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

        public override Song GetById(long id)
        {
            var song = _context.Songs
                .Include(i => i.Album)
                .Include(i => i.Artist)
                .Include(i => i.Genre)
                .SingleOrDefault(i => i.Id == id);
            if (song == null)
            {
                throw new ElementNotFoundException();
            }

            return song;
        }

        public PagedList<SongDto> GetSongsByCriteriaPaged(string? filter, PaginationParameters paginationParameters)
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

            return PagedList<SongDto>.ToPagedList(songsDto, paginationParameters.PageNumber,
                paginationParameters.PageSize);
        }


        public override Song Insert(Song song)
        {
            _context.Songs.Add(song);
            _context.SaveChanges();
            _context.Entry(song).Reference(i => i.Genre).Load();
            _context.Entry(song).Reference(i => i.Album).Load();
            _context.Entry(song).Reference(i => i.Artist).Load();
            return song;
        }

        public override Song Update(Song song)
        {
            _context.Songs.Update(song);
            _context.SaveChanges();
            _context.Entry(song).Reference(i => i.Genre).Load();
            _context.Entry(song).Reference(i => i.Album).Load();
            _context.Entry(song).Reference(i => i.Artist).Load();
            return song;
        }
    }
}