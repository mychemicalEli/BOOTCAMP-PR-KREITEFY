using api.Application.Dtos;
using api.Application.Services.Interfaces;
using framework.Infrastructure.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;

[Route("[controller]")]
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
    public ActionResult<UserDto> GetAllUsersWithRoleName()
    {
        return Ok(_service.GetAllUsersWithRoleName());
    }

    [HttpPost]
    public override ActionResult<UserDto> Insert(UserDto userDto)
    {
        try
        {
            var response = _service.Insert(userDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut]
    public override ActionResult<UserDto> Update(UserDto userDto)
    {
        try
        {
            var response = _service.Update(userDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}