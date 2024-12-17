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
    
    [NonAction]
    public override ActionResult<UserDto> Get(long id)
    {
        throw new NotImplementedException();
    }

    
    [NonAction]
    public override ActionResult<UserDto> Insert(UserDto userDto)
    {
        throw new NotImplementedException();
    }

    [NonAction]
    public override ActionResult<UserDto> Update(UserDto userDto)
    {
        throw new NotImplementedException();
    }


    [HttpGet]
    [Produces("application/json")]
    public async Task<ActionResult<UserDto>> GetAllUsersWithRoleName()
    {
        try
        {
            var response = await _service.GetAllUsersWithRoleNameAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetByIdAsync(long id)
    {
        try
        {
            var user = await _service.GetByIdAsync(id); // Asume que la lógica asincrónica está en el servicio.
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPost]
    public async Task<ActionResult<UserDto>> InsertAsync([FromBody] UserDto userDto)
    {
        try
        {
            var response = await _service.InsertAsync(userDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut]
    public async Task<ActionResult<UserDto>> UpdateAsync([FromBody] UserDto userDto)
    {
        try
        {
            var response = await _service.UpdateAsync(userDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}