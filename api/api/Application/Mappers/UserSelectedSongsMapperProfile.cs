using api.Application.Dtos;
using api.Domain.Entities;
using AutoMapper;

namespace api.Application.Mappers;

public class UserSelectedSongsMapperProfile :Profile
{
    public UserSelectedSongsMapperProfile()
    {
        CreateMap<Song, SongsForYouDto>()
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Artist.Name))
            .ForMember(dest => dest.AlbumCover, opt => opt.MapFrom(src => Convert.ToBase64String(src.Album.Cover)));
        CreateMap<SongsForYouDto, Song>();
    }
}