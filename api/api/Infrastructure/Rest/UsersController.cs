using api.Application.Dtos;
using api.Application.Services.Interfaces;
using framework.Infrastructure.Rest;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;

[Route("api/[controller]")]
[ApiController]
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
    public ActionResult<UserDto> GetAllUsersWithRoleName()
    {
        return Ok(_service.GetAllUsersWithRoleName());
    }
}