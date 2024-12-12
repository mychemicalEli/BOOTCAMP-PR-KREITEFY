using api.Application.Mappers;

public static class MapperProfile
{
    public static IServiceCollection AddMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(RoleMapperProfile));
        services.AddAutoMapper(typeof(UserMapperProfile));
        services.AddAutoMapper(typeof(ArtistMapperProfile));
        services.AddAutoMapper(typeof(GenreMapperProfile));
        services.AddAutoMapper(typeof(AlbumMapperProfile));
        services.AddAutoMapper(typeof(SongMapperProfile));
        services.AddAutoMapper(typeof(UserSongsMapperProfile));
        services.AddAutoMapper(typeof(RatingMapperProfile));
        services.AddAutoMapper(typeof(UserSelectedSongsMapperProfile));
        return services;
    }
}