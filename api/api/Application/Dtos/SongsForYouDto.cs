namespace api.Application.Dtos;

public class SongsForYouDto
{
    public string GenreName { get; set; }
    public IEnumerable<UserSelectedSongsDto> TopSongs { get; set; }
}