using framework.Application.Dtos;

namespace api.Application.Dtos;

public class AlbumDto : IDto
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required byte[] Cover { get; set; }

}