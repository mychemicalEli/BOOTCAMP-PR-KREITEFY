using framework.Application.Dtos;

namespace api.Application.Dtos;

public class RatingDto : IDto
{
    public long Id { get; set; }
    public required long UserId { get; set; }
    public required long SongId { get; set; }
    public  required decimal Stars { get; set; }
}