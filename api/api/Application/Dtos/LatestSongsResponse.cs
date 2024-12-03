using api.Infrastructure.Persistence;
using Humanizer;

namespace api.Application.Dtos;

public class LatestSongsResponse
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required string ArtistName { get; set; }
    public required string AlbumCover { get; set; }
    public required DateTime AddedAt { get; set; }
    public string? HumanizedAddedAt { get; set; }
}