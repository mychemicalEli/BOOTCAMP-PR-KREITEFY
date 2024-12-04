using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Persistence;
using AutoMapper;
using framework.Application.Services;

namespace api.Application.Services.Implementations;

public class UserService : GenericService<User, UserDto>, IUserService
{
    private readonly IUserRepository _songRepository;

    public UserService(IUserRepository songRepository, IMapper mapper) : base(songRepository, mapper)
    {
        _songRepository = songRepository;
    }

    public List<UserDto> GetAllUsersWithRoleName()
    {
        return _songRepository.GetAllUsersWithRoleName();
    }

    public UserDto RegisterUser(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        var newUser = _songRepository.Insert(user);
        return _mapper.Map<UserDto>(newUser);
    }
    
    public UserDto GetUserByEmail(string email)
    {
        var user = _songRepository.GetUserByEmail(email);  
        return _mapper.Map<UserDto>(user); 
    }
}