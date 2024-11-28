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
    private readonly ISongRepository _repository;

    public SongService(ISongRepository repository, IMapper mapper) : base(repository,
        mapper)
    {
        _repository = repository;
    }

    public PagedList<SongDto> GetSongsByCriteriaPaged(string? filter, PaginationParameters paginationParameters)
    {
        var songs = _repository.GetSongsByCriteriaPaged(filter, paginationParameters);
        return songs;
    }
}