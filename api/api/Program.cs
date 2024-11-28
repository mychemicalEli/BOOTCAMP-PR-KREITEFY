using api.Application.Mappers;
using api.Application.Services.Implementations;
using api.Application.Services.Interfaces;
using api.Domain.Persistence;
using api.Infrastructure.Persistence;
using framework.Domain.Persistence;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();

builder.Services.AddAutoMapper(typeof(RoleMapperProfile));
builder.Services.AddAutoMapper(typeof(UserMapperProfile));
builder.Services.AddAutoMapper(typeof(ArtistMapperProfile));
builder.Services.AddAutoMapper(typeof(GenreMapperProfile));
builder.Services.AddAutoMapper(typeof(AlbumMapperProfile));

builder.Services.AddScoped<IImageVerifier, ImageVerifier>();

builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<KreitekfyContext>(options =>
        options.UseInMemoryDatabase(connectionString));
}

var app = builder.Build();
ConfigureExceptionhandler(app);

if (builder.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<KreitekfyContext>();
    DataLoader dataLoader = new(context);
    dataLoader.LoadData();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void ConfigureExceptionhandler(WebApplication app)
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            IExceptionHandlerPathFeature? exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            if (exceptionHandlerPathFeature?.Error != null)
            {
                logger.LogError(exceptionHandlerPathFeature.Error,
                    "An unhandled exception occurred while processing the request");
            }
            else
            {
                logger.LogError("An unhandled exception occurred while processing the request.");
            }

            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An error occurred while processing your request");
        });
    });
}