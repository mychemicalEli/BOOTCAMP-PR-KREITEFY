using api.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;


public class UserSongsController:ControllerBase
{
    private readonly IUserSongsService _userSongsService;
    public UserSongsController(IUserSongsService userSongsService)
    {
        _userSongsService = userSongsService;
    }
    [HttpPost("api/play")]
    [Produces("application/json")]
    [Authorize]
    public IActionResult IncrementStreams([FromQuery] long userId, [FromQuery] long songId)
    {
        try
        {
            _userSongsService.IncrementStreams(userId, songId);
            return Ok(new { Message = "Stream count updated successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}