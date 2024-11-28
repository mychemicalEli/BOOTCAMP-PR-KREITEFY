using api.Application.Dtos;
using api.Application.Services.Interfaces;
using framework.Application;
using framework.Infrastructure.Rest;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;

[Route("api/[controller]")]
[ApiController]
public class SongsController : GenericCrudController<SongDto>
{
    private ISongService _service;

    public SongsController(ISongService service) : base(service)
    {
        _service = service;
    }

    [NonAction]
    public override ActionResult<IEnumerable<SongDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Produces("application/json")]
    public ActionResult<PagedResponse<SongDto>> Get([FromQuery] string? filter,
        [FromQuery] PaginationParameters paginationParameters)
    {
        try
        {
            PagedList<SongDto> page = _service.GetSongsByCriteriaPaged(filter, paginationParameters);
            var response = new PagedResponse<SongDto>
            {
                CurrentPage = page.CurrentPage,
                TotalPages = page.TotalPages,
                PageSize = page.PageSize,
                TotalCount = page.TotalCount,
                Data = page
            };
            return Ok(response);
        }
        catch (MalformedFilterException)
        {
            return BadRequest();
        }
    }
}