using api.Application.Dtos;
using api.Domain.Entities;
using AutoMapper;

namespace api.Application.Mappers;

public class RoleMapperProfile : Profile
{
    public RoleMapperProfile()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
    }
}