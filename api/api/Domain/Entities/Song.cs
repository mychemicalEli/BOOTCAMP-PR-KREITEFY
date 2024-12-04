using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Domain.Entities;

[Table("songs")]
public class Song
{
    public long Id { get; set; }
    [MaxLength(255)] public required string Title { get; set; }
    public required long ArtistId { get; set; }
    public Artist Artist { get; set; }
    public required long AlbumId { get; set; }
    public Album Album { get; set; }
    public required long GenreId { get; set; }
    public Genre Genre { get; set; }
    public required TimeSpan Duration { get; set; }
    [Range(1, 4)] public required int MediaRating { get; set; }
    [Range(0, long.MaxValue)] public required long Streams { get; set; }
    public required DateTime AddedAt { get; set; } = DateTime.Now;
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<UserSongs> UserSongs { get; set; }
}