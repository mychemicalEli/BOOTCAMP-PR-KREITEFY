namespace api.Application.Dtos;

public class UserSelectedSongsDto
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required string ArtistName { get; set; }
    public required string AlbumCover { get; set; }
    public required string GenreName { get; set; }
    public required long Streams { get; set; }
    public required int MediaRating { get; set; }
}