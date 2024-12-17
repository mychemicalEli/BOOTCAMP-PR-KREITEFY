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
    
    public async Task<SongDto> GetSongByIdAsync(long id)
    {
        var song = await _songRepository.GetByIdAsync(id);
        if (song == null)
        {
            return null;
        }

        var songDto = _mapper.Map<SongDto>(song);
        return songDto;
    }


    public async Task<PagedList<SongDto>> GetSongsByCriteriaPagedAsync(string? filter, PaginationParameters paginationParameters)
    {
        var songs = await _songRepository.GetSongsByCriteriaPagedAsync(filter, paginationParameters);
        return songs;
    }
    
    public async Task<IEnumerable<LatestSongsResponse>> GetLatestSongsAsync(int count = 5, long? genreId = null)
    {
        var latestSongs = await _songRepository.GetLatestSongsAsync(count, genreId);
        return latestSongs.Select(song =>
        {
            song.HumanizedAddedAt = _dateHumanizer.HumanizeDate(song.AddedAt);
            return song;
        });
    }
    
    public async Task<IEnumerable<MostPlayedSongsDto>> GetMostPlayedSongsAsync(int count = 5, long? genreId = null)
    {
        var mostPlayedSongs = await _songRepository.GetMostPlayedSongsAsync(count, genreId);
        return mostPlayedSongs;
    }
    
    public async Task<SongDto> InsertAsync(SongDto songDto)
    {
        var song = _mapper.Map<Song>(songDto);
        await _songRepository.InsertAsync(song);
        var insertedSongDto = _mapper.Map<SongDto>(song);
        return insertedSongDto;
    }
    
    public async Task<SongDto> UpdateAsync(long id, SongDto songDto)
    {
        var song = await _songRepository.GetByIdAsync(id);
        if (song == null)
        {
            return null;
        }
        _mapper.Map(songDto, song);
        await _songRepository.UpdateAsync(song);
        var updatedSongDto = _mapper.Map<SongDto>(song);
        return updatedSongDto;
    }
}