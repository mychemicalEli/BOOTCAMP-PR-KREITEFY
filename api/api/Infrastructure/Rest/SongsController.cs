using api.Application.Dtos;
using api.Application.Services.Interfaces;
using framework.Application;
using framework.Infrastructure.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;

[Route("[controller]")]
[ApiController]
[Authorize]
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
    [Authorize]
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

    [HttpGet("latest")]
    [Produces("application/json")]
    [Authorize]
    public ActionResult<IEnumerable<LatestSongsResponse>> GetLatestSongs([FromQuery] int count = 5, [FromQuery] long? genreId = null)
    {
        try
        {
            var songs = _service.GetLatestSongs(count, genreId);
            return Ok(songs);
        }
        catch (Exception ex)
        {
            return BadRequest("Error fetching latest songs: " + ex.Message);
        }
    }
}