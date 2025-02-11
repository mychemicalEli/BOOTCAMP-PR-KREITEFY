using framework.Application.Dtos;

namespace api.Application.Dtos;

public class UserSongsDto : IDto
{
    public long Id { get; set; }
    public required long UserId { get; set; }
    public required long SongId { get; set; }
    public DateTime? LastPlayedAt { get; set; }
    public required long TotalStreams { get; set; }
    public SongsForYouDto SongForYou { get; set; }
}