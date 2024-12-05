using api.Application.Dtos;
using api.Application.Services.Interfaces;
using framework.Infrastructure.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;
[Route("[controller]")]
[ApiController]
[Authorize]
public class GenresController:GenericCrudController<GenreDto>
{
    public GenresController(IGenreService service) : base(service)
    {
    }
}