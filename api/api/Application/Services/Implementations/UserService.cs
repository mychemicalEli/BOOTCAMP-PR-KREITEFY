using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Persistence;
using AutoMapper;
using framework.Application.Services;

namespace api.Application.Services.Implementations;

public class UserService : GenericService<User, UserDto>, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, IMapper mapper) : base(userRepository, mapper)
    {
        _userRepository = userRepository;
    }

    public List<UserDto> GetAllUsersWithRoleName()
    {
        return _userRepository.GetAllUsersWithRoleName();
    }

    public UserDto RegisterUser(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        var newUser = _userRepository.Insert(user);
        return _mapper.Map<UserDto>(newUser);
    }
    
    public UserDto GetUserByEmail(string email)
    {
        var user = _userRepository.GetUserByEmail(email);  
        return _mapper.Map<UserDto>(user); 
    }
}