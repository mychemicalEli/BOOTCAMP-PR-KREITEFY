using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Persistence;
using AutoMapper;
using framework.Application;
using framework.Application.Services;

namespace api.Application.Services.Implementations;

public class SongService : GenericService<Song, SongDto>, ISongService
{
    private readonly ISongRepository _songRepository;
    private readonly IDateHumanizer _dateHumanizer;

    public SongService(ISongRepository songRepository, IMapper mapper, IDateHumanizer dateHumanizer) : base(songRepository,
        mapper)
    {
        _songRepository = songRepository;
        _dateHumanizer = dateHumanizer;
    }

    public PagedList<SongDto> GetSongsByCriteriaPaged(string? filter, PaginationParameters paginationParameters)
    {
        var songs = _songRepository.GetSongsByCriteriaPaged(filter, paginationParameters);
        return songs;
    }
    
    public IEnumerable<LatestSongsResponse> GetLatestSongs(int count = 5, long? genreId = null)
    {
        var latestSongs = _songRepository.GetLatestSongs(count, genreId);
        return latestSongs.Select(song =>
        {
            song.HumanizedAddedAt = _dateHumanizer.HumanizeDate(song.AddedAt); 
            return song;
        });
    }
    
    public IEnumerable<MostPlayedSongsDto> GetMostPlayedSongs(int count = 5, long? genreId = null)
    {
        var mostPlayedSongs = _songRepository.GetMostPlayedSongs(count, genreId);
        return mostPlayedSongs;
    }
}