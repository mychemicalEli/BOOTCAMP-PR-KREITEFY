using api.Application.Dtos;
using api.Domain.Entities;
using AutoMapper;

namespace api.Application.Mappers;

public class UserSongsMapperProfile : Profile
{
    public UserSongsMapperProfile()
    {
        CreateMap<UserSongs, UserSongsDto>()
            .ForMember(dest => dest.Song, opt => opt.MapFrom(src => src.Song));
    }
}