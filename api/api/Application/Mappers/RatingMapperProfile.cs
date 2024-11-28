using api.Application.Dtos;
using api.Domain.Entities;
using AutoMapper;

namespace api.Application.Mappers;

public class RatingMapperProfile : Profile
{
    public RatingMapperProfile()
    {
        CreateMap<Rating, RatingDto>().ReverseMap();
    }
}