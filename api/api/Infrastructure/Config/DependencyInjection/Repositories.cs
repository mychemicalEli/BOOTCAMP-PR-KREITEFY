using api.Domain.Persistence;
using api.Infrastructure.Persistence;

public static class Repositories
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IArtistRepository, ArtistRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IAlbumRepository, AlbumRepository>();
        services.AddScoped<ISongRepository, SongRepository>();
        services.AddScoped<IUserSongsRepository, UserSongsRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();
        return services;
    }
}