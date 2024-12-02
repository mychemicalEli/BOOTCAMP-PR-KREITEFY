using Humanizer;

namespace api.Application.Dtos;

public class LatestSongsRequest
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required long ArtistId { get; set; }
    public required string ArtistName { get; set; }
    public required long AlbumId { get; set; }
    public required string AlbumCover { get; set; }
    public required long GenreId { get; set; }
    public required string GenreName { get; set; }
    private DateTime _addedAt;
    public required DateTime AddedAt
    {
        get => _addedAt;
        set
        {
            _addedAt = value;
            HumanizedAddedAt = _addedAt.Humanize();
        }
    }
    public string? HumanizedAddedAt { get; private set; }
}