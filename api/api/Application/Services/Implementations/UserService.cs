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

    public UserDto RegisterUser(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        var newUser = _repository.Insert(user);
        return _mapper.Map<UserDto>(newUser);
    }
    
    public UserDto GetUserByEmail(string email)
    {
        var user = _repository.GetUserByEmail(email);  
        return _mapper.Map<UserDto>(user); 
    }
}