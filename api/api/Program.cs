using System.Text;
using api.Application.Mappers;
using api.Application.Services.Implementations;
using api.Application.Services.Interfaces;
using api.Domain.Persistence;
using api.Infrastructure.Persistence;
using api.Infrastructure.Services;
using framework.Domain.Persistence;
using framework.Infrastructure.Specs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
//Services
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IUserSongsService, UserSongsService>();
builder.Services.AddScoped<IRatingService, RatingService>();

//Repositories
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IUserSongsRepository, UserSongsRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();

//Mappers
builder.Services.AddAutoMapper(typeof(RoleMapperProfile));
builder.Services.AddAutoMapper(typeof(UserMapperProfile));
builder.Services.AddAutoMapper(typeof(ArtistMapperProfile));
builder.Services.AddAutoMapper(typeof(GenreMapperProfile));
builder.Services.AddAutoMapper(typeof(AlbumMapperProfile));
builder.Services.AddAutoMapper(typeof(SongMapperProfile));
builder.Services.AddAutoMapper(typeof(UserSongsMapperProfile));
builder.Services.AddAutoMapper(typeof(UserSelectedSongsMapperProfile));

builder.Services.AddScoped<IImageVerifier, ImageVerifier>();
builder.Services.AddScoped<IDateHumanizer, DateHumanizer>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped(typeof(ISpecificationParser<>), typeof(SpecificationParser<>));

builder.Services.AddHttpContextAccessor();
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

builder.Services.AddAuthorization();
var app = builder.Build();
ConfigureExceptionhandler(app);
app.UseCors("AllowFrontEnd");

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
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers().RequireAuthorization();

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