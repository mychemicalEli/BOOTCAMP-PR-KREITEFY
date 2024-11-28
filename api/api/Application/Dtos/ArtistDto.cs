using framework.Application.Dtos;

namespace api.Application.Dtos;

public class ArtistDto : IDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
}