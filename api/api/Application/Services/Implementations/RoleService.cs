using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Persistence;
using AutoMapper;
using framework.Application.Services;

namespace api.Application.Services.Implementations;

public class RoleService : GenericService<Role, RoleDto>, IRoleService
{
    public RoleService(IRoleRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}