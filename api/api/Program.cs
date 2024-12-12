using api.Infrastructure.Config.Middleware;
using api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Swagger
builder.Services.AddSwaggerDocumentation();

// CORS
builder.Services.AddCorsPolicies();

// Application Services
builder.Services.AddApplicationServices();

// Repositories
builder.Services.AddRepositories();

// External Services
builder.Services.AddExternalServices();

// Framework Services
builder.Services.AddFrameworkServices();

// Mapper Profiles
builder.Services.AddMapperProfiles();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<KreitekfyContext>(options =>
        options.UseInMemoryDatabase(connectionString));
}

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowFrontEnd");

if (app.Environment.IsDevelopment())
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
