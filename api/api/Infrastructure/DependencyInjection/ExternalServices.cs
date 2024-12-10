using api.Application.Services.Interfaces;
using api.Domain.Persistence;
using api.Infrastructure.Persistence;
using api.Infrastructure.Services;

public static class ExternalServices
{
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddScoped<IDateHumanizer, DateHumanizer>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        return services;
    }
}