using framework.Application.Dtos;

namespace api.Application.Dtos;

public class MostPlayedSongsDto: IDto
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required string ArtistName { get; set; }
    public required string AlbumCover { get; set; }
    public required long Streams { get; set; }
}