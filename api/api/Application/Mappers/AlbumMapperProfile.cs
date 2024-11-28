using api.Application.Dtos;
using api.Domain.Entities;
using AutoMapper;

namespace api.Application.Mappers;

public class AlbumMapperProfile : Profile
{
    public AlbumMapperProfile()
    {
        CreateMap<Album, AlbumDto>().ReverseMap();
    }
}