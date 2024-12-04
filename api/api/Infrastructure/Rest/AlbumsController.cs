using api.Application.Dtos;
using api.Application.Services.Interfaces;
using framework.Domain.Persistence;
using framework.Infrastructure.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AlbumsController : GenericCrudController<AlbumDto>
{
    private readonly ILogger<AlbumsController> _logger;

    public AlbumsController(IAlbumService service, ILogger<AlbumsController> logger) : base(service)
    {
        _logger = logger;
    }


    public override ActionResult<AlbumDto> Insert(AlbumDto dto)
    {
        try
        {
            return base.Insert(dto);
        }
        catch (InvalidImageException)
        {
            _logger.LogInformation("Invalid image inserting album with {dto.Title} name", dto.Title);
            return BadRequest();
        }
    }

    public override ActionResult<AlbumDto> Update(AlbumDto dto)
    {
        try
        {
            return base.Update(dto);
        }
        catch (InvalidImageException)
        {
            _logger.LogInformation("Invalid image updating album with {dto.Id} ID", dto.Id);
            return BadRequest();
        }
    }
}