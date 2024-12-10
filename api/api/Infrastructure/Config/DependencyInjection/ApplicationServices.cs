using api.Application.Services.Implementations;
using api.Application.Services.Interfaces;

public static class ApplicationService
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IArtistService, ArtistService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IAlbumService, AlbumService>();
        services.AddScoped<ISongService, SongService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserSongsService, UserSongsService>();
        services.AddScoped<IRatingService, RatingService>();
        return services;
    }
}