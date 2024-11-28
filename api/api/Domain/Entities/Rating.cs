using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Domain.Entities;

[Table("ratings")]
public class Rating
{
    public long Id { get; set; }
    public required long UserId { get; set; }
    public User User { get; set; }
    public required long SongId { get; set; }
    public Song Song { get; set; }
    [Range(1, 4)] public required decimal Stars { get; set; }
}