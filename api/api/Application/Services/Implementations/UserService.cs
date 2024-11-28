using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Persistence;
using AutoMapper;
using framework.Application.Services;

namespace api.Application.Services.Implementations;

public class UserService : GenericService<User, UserDto>, IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
    }

    public List<UserDto> GetAllUsersWithRoleName()
    {
        return _repository.GetAllUsersWithRoleName();
    }
}