using framework.Application.Dtos;

namespace api.Application.Dtos;

public class SongDto : IDto
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public  required long ArtistId { get; set; }
    public required string ArtistName { get; set; }
    public required long AlbumId { get; set; }
    public required string AlbumName { get; set; }
    public required string AlbumCover { get; set; }
    public required long GenreId { get; set; }
    public required string GenreName { get; set; }
    public required string Duration { get; set; }
    public required decimal MediaRating { get; set; }
    public required long Streams { get; set; }
    public required DateTime AddedAt { get; set; }
}