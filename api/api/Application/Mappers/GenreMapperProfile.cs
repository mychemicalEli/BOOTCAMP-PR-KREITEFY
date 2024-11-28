using api.Application.Dtos;
using api.Domain.Entities;
using AutoMapper;

namespace api.Application.Mappers;

public class GenreMapperProfile : Profile
{
    public GenreMapperProfile()
    {
        CreateMap<Genre, GenreDto>().ReverseMap();
    }
}