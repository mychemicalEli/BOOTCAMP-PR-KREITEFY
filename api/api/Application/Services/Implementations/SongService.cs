using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Persistence;
using AutoMapper;
using framework.Application;
using framework.Application.Services;
using framework.Domain.Persistence;

namespace api.Application.Services.Implementations;

public class SongService : GenericService<Song, SongDto>, ISongService
{
    private readonly ISongRepository _repository;
    private readonly IDateHumanizer _dateHumanizer;

    public SongService(ISongRepository repository, IMapper mapper, IDateHumanizer dateHumanizer) : base(repository,
        mapper)
    {
        _repository = repository;
        _dateHumanizer = dateHumanizer;
    }

    public PagedList<SongDto> GetSongsByCriteriaPaged(string? filter, PaginationParameters paginationParameters)
    {
        var songs = _repository.GetSongsByCriteriaPaged(filter, paginationParameters);
        return songs;
    }
    
    public IEnumerable<LatestSongsResponse> GetLatestSongs(int count = 5)
    {
        var latestSongs = _repository.GetLatestSongs(count);
        return latestSongs.Select(song =>
        {
            song.HumanizedAddedAt = _dateHumanizer.HumanizeDate(song.AddedAt);
            return song;
        });
    }

    public void IncrementStreams(long songId)
    {
        var song = _repository.GetById(songId);
        if (song == null)
        {
            throw new ElementNotFoundException($"The song with id {songId} was not found.");
        }

        song.Streams += 1;
        _repository.Update(song);
    }
}