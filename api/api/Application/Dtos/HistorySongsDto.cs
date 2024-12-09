namespace api.Application.Dtos;

public class HistorySongsDto
{
    public DateTime LastPlayedAt { get; set; }
    public string HumanizedPlayedAt { get; set; }
    public long SongId { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
}