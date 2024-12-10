using framework.Domain.Persistence;
using framework.Infrastructure.Specs;

public static class FrameworkServices
{
    public static IServiceCollection AddFrameworkServices(this IServiceCollection services)
    {
        services.AddScoped<IImageVerifier, ImageVerifier>();
        services.AddScoped(typeof(ISpecificationParser<>), typeof(SpecificationParser<>));
        return services;
    }
}