using api.Application.Dtos;
using api.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;

[Route("[controller]")]
[ApiController]
[Authorize]
public class RatingController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpPost]
    [Produces("application/json")]
    [Authorize]
    public IActionResult AddRating([FromBody] RatingDto ratingDto)
    {
        try
        {
            _ratingService.AddRating(ratingDto);
            return Ok(new { message = "Rating added succesfuly." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = "Error adding rating: " + ex.Message });
        }
    }
}