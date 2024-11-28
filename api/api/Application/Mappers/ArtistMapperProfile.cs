using api.Application.Dtos;
using api.Domain.Entities;
using AutoMapper;

namespace api.Application.Mappers;

public class ArtistMapperProfile : Profile
{
    public ArtistMapperProfile()
    {
        CreateMap<Artist, ArtistDto>().ReverseMap();
    }
}