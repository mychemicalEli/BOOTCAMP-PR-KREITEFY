using api.Application.Dtos;
using api.Application.Services.Interfaces;
using framework.Infrastructure.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : GenericCrudController<UserDto>
{
    private IUserService _service;

    public UsersController(IUserService service) : base(service)
    {
        _service = service;
    }

    [NonAction]
    public override ActionResult<IEnumerable<UserDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Produces("application/json")]
    [Authorize]
    public ActionResult<UserDto> GetAllUsersWithRoleName()
    {
        return Ok(_service.GetAllUsersWithRoleName());
    }
}