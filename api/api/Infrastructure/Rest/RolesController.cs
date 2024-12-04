using api.Application.Dtos;
using api.Application.Services.Interfaces;
using framework.Infrastructure.Rest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Infrastructure.Rest;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolesController : GenericCrudController<RoleDto>
{
    public RolesController(IRoleService service) : base(service)
    {
    }
}