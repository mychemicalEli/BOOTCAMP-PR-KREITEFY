using api.Application.Dtos;
using api.Application.Services.Interfaces;
using framework.Infrastructure.Rest;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;

[Route("api/[controller]")]
[ApiController]
public class RolesController : GenericCrudController<RoleDto>
{
    public RolesController(IRoleService service) : base(service)
    {
    }
}