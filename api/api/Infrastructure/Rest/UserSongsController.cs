using api.Application.Dtos;
using api.Application.Services.Interfaces;
using framework.Application;
using framework.Infrastructure.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;

public class UserSongsController : ControllerBase
{
    private readonly IUserSongsService _userSongsService;

    public UserSongsController(IUserSongsService userSongsService)
    {
        _userSongsService = userSongsService;
    }

    [HttpPost("play")]
    [Produces("application/json")]
    [Authorize]
    public async Task<IActionResult> IncrementStreamsAsync([FromQuery] long userId, [FromQuery] long songId)
    {
        try
        {
            await _userSongsService.IncrementStreamsAsync(userId, songId);
            return Ok(new { Message = "Stream count updated successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("user/{userId}/songsforyou")]
    [Produces("application/json")]
    [Authorize]
    public async Task<IActionResult> GetSongsForYouAsync([FromRoute] long userId)
    {
        try
        {
            var songsForYou = await _userSongsService.GetSongsForYouAsync(userId);
            if (songsForYou == null || !songsForYou.Any())
            {
                return NotFound(new { Message = "No personalized songs found for this user." });
            }

            return Ok(songsForYou);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("user/{userId}/history")]
    [Produces("application/json")]
    [Authorize]
    public async Task<ActionResult<PagedResponse<HistorySongsDto>>> GetHistorySongsAsync([FromRoute] long userId,
        [FromQuery] PaginationParameters paginationParameters)
    {
        try
        {
            var page = await _userSongsService.GetHistorySongsAsync(userId, paginationParameters);
            var response = new PagedResponse<HistorySongsDto>
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