using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Domain.Entities;

[Table("user_songs")]
public class UserSongs
{
    public long Id { get; set; }
    [Required] public required long UserId { get; set; }
    public User User { get; set; }
    [Required] public required long SongId { get; set; }
    public Song Song { get; set; }
    public DateTime? LastPlayedAt { get; set; }

    [Required] public required long TotalStreams { get; set; }
}