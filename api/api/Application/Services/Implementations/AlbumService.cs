using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Persistence;
using AutoMapper;
using framework.Application.Services;
using framework.Domain.Persistence;

namespace api.Application.Services.Implementations;

public class AlbumService : GenericService<Album, AlbumDto>, IAlbumService
{
    private readonly IImageVerifier _imageVerifier;

    public AlbumService(IAlbumRepository repository, IMapper mapper, IImageVerifier imageVerifier) : base(repository,
        mapper)
    {
        _imageVerifier = imageVerifier;
    }


    public override AlbumDto Insert(AlbumDto dto)
    {
        if (!_imageVerifier.IsImage(dto.Cover))
            throw new InvalidImageException();
        return base.Insert(dto);
    }

    public override AlbumDto Update(AlbumDto dto)
    {
        if (!_imageVerifier.IsImage(dto.Cover))
            throw new InvalidImageException();
        return base.Update(dto);
    }
}