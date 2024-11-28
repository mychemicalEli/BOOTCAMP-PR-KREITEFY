using api.Application.Dtos;
using api.Domain.Entities;
using AutoMapper;

namespace api.Application.Mappers;

public class SongMapperProfile : Profile
{
    public SongMapperProfile()
    {
        CreateMap<Song, SongDto>()
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Artist.Name))
            .ForMember(dest => dest.AlbumName, opt => opt.MapFrom(src => src.Album.Title))
            .ForMember(dest => dest.AlbumCover, opt => opt.MapFrom(src => Convert.ToBase64String(src.Album.Cover)))
            .ForMember(dest => dest.Duration,
                opt => opt.MapFrom(src => TimeSpanConverter.ToString(src.Duration)));

        CreateMap<SongDto, Song>()
            .ForMember(dest => dest.Duration,
                opt => opt.MapFrom(src => TimeSpanConverter.ToTimeSpan(src.Duration)));
    }
}