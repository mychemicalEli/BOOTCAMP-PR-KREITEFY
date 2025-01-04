using api.Application.Dtos;
using api.Application.Exceptions;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Exceptions;
using api.Domain.Persistence;
using api.Domain.Validators;
using AutoMapper;
using framework.Application.Services;
using framework.Domain.Persistence;

namespace api.Application.Services.Implementations;

public class UserService : GenericService<User, UserDto>, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRoleRepository _roleRepository;

    public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher,
        IRoleRepository roleRepository) : base(
        userRepository, mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _roleRepository = roleRepository;
    }

    public async Task<List<UserDto>> GetAllUsersWithRoleNameAsync()
    {
        return await _userRepository.GetAllUsersWithRoleNameAsync();
    }

    public async Task<UserDto> RegisterUserAsync(UserDto userDto)
    {
        ValidateEmailAndPassword(userDto);
        ValidateRole(userDto);

        userDto.Password = _passwordHasher.HashPassword(userDto.Password);
        var user = _mapper.Map<User>(userDto);
        var newUser = await _userRepository.InsertAsync(user);
        return _mapper.Map<UserDto>(newUser);
    }

    public async Task<UserDto> GetByIdAsync(long id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new ElementNotFoundException("User not found."); 
        }

        return _mapper.Map<UserDto>(user);
    }
    
    public async Task<UserDto> UpdateAsync(UserDto userDto)
    {
       
        ValidateRole(userDto);

        if (!string.IsNullOrEmpty(userDto.Password))
        {
            ValidateEmailAndPassword(userDto);
            userDto.Password = _passwordHasher.HashPassword(userDto.Password);
        }
        else
        {
            var existingPassword = await _userRepository.HandlePasswordUpdateAsync(userDto.Id);
            userDto.Password = existingPassword;
        }
        
        var user = _mapper.Map<User>(userDto);
        var updatedUser = await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserDto>(updatedUser);
    }

    public async Task<UserDto> InsertAsync(UserDto userDto)
    {
        ValidateEmailAndPassword(userDto);
        ValidateRole(userDto);

        if (!string.IsNullOrEmpty(userDto.Password))
        {
            userDto.Password = _passwordHasher.HashPassword(userDto.Password);
        }

        var user = _mapper.Map<User>(userDto);
        var newUser = await _userRepository.InsertAsync(user); 
        return _mapper.Map<UserDto>(newUser);
    }


    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        return _mapper.Map<UserDto>(user);
    }


    private void ValidateEmailAndPassword(UserDto userDto)
    {
        if (!UserValidator.IsValidEmail(userDto.Email))
        {
            throw new InvalidEmailOrPasswordException("Invalid email.");
        }

        if (!UserValidator.IsValidPassword(userDto.Password))
        {
            throw new InvalidEmailOrPasswordException("The password does not meet the security requirements.");
        }
    }
    
    private async Task ValidateRole(UserDto userDto)
    {
        if (!await _roleRepository.ExistsAsync(userDto.RoleId))
        {
            throw new InvalidRoleException("Invalid role ID.");
        }
    }
}