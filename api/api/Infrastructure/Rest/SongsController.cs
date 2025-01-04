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
    
    [NonAction]
    public override ActionResult<SongDto> Get(long id)
    {
        throw new NotImplementedException();
    }
    
    [NonAction]
    public override ActionResult<SongDto> Insert(SongDto songDto)
    {
        throw new NotImplementedException();
    }

    [NonAction]
    public override ActionResult<SongDto> Update(SongDto songDto)
    {
        throw new NotImplementedException();
    }

    
    [HttpGet("{id}")]
    [Produces("application/json")]
    [Authorize]
    public async Task<ActionResult<SongDto>> GetSongById(long id)
    {
        try
        {
            var song = await _service.GetSongByIdAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            return Ok(song);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error fetching song: {ex.Message}");
        }
    }

    [HttpGet]
    [Produces("application/json")]
    [Authorize]
    public async Task<ActionResult<PagedResponse<SongDto>>> Get([FromQuery] string? filter, [FromQuery] PaginationParameters paginationParameters)
    {
        try
        {
            var page = await _service.GetSongsByCriteriaPagedAsync(filter, paginationParameters);
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
    public async Task<ActionResult<IEnumerable<LatestSongsResponse>>> GetLatestSongs([FromQuery] int count = 5, [FromQuery] long? genreId = null)
    {
        try
        {
            var songs = await _service.GetLatestSongsAsync(count, genreId);
            return Ok(songs);
        }
        catch (Exception ex)
        {
            return BadRequest("Error fetching latest songs: " + ex.Message);
        }
    }
    
    [HttpGet("most-played")]
    [Produces("application/json")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<MostPlayedSongsDto>>> GetMostPlayedSongs([FromQuery] int count = 5, [FromQuery] long? genreId = null)
    {
        try
        {
            var songs = await _service.GetMostPlayedSongsAsync(count, genreId);
            return Ok(songs);
        }
        catch (Exception ex)
        {
            return BadRequest("Error fetching most played songs: " + ex.Message);
        }
    }
    
    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<SongDto>> InsertAsync([FromBody] SongDto songDto)
    {
        try
        {
            var insertedSong = await _service.InsertAsync(songDto);
            return CreatedAtAction(nameof(GetSongById), new { id = insertedSong.Id }, insertedSong);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error inserting song: {ex.Message}");
        }
    }

 
    [HttpPut("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<SongDto>> UpdateAsync(long id, [FromBody] SongDto songDto)
    {
        try
        {
            var updatedSong = await _service.UpdateAsync(id, songDto);
            if (updatedSong == null)
            {
                return NotFound();
            }
            return Ok(updatedSong);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error updating song: {ex.Message}");
        }
    }
}