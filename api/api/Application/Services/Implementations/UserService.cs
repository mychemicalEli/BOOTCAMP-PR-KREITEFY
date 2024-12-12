using api.Application.Dtos;
using api.Application.Exceptions;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Exceptions;
using api.Domain.Persistence;
using api.Domain.Validators;
using AutoMapper;
using framework.Application.Services;

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

    public List<UserDto> GetAllUsersWithRoleName()
    {
        return _userRepository.GetAllUsersWithRoleName();
    }

    public UserDto RegisterUser(UserDto userDto)
    {
        ValidateEmailAndPassword(userDto);
        ValidateRole(userDto);

        userDto.Password = _passwordHasher.HashPassword(userDto.Password);
        var user = _mapper.Map<User>(userDto);
        var newUser = _userRepository.Insert(user);
        return _mapper.Map<UserDto>(newUser);
    }

    public override UserDto Update(UserDto userDto)
    {
       
        ValidateRole(userDto);

        if (!string.IsNullOrEmpty(userDto.Password))
        {
            ValidateEmailAndPassword(userDto);
            userDto.Password = _passwordHasher.HashPassword(userDto.Password);
        }
        else
        {
            // Si no se pasa una nueva contraseña, mantener la contraseña existente
            var existingPassword = _userRepository.HandlePasswordUpdate(userDto.Id);
            userDto.Password = existingPassword;
        }
        
        var user = _mapper.Map<User>(userDto);
        var updatedUser = _userRepository.Update(user);
        return _mapper.Map<UserDto>(updatedUser);
    }

    public override UserDto Insert(UserDto userDto)
    {
        ValidateEmailAndPassword(userDto);
        ValidateRole(userDto);

        if (!string.IsNullOrEmpty(userDto.Password))
        {
            userDto.Password = _passwordHasher.HashPassword(userDto.Password);
        }

        var user = _mapper.Map<User>(userDto);
        var newUser = _userRepository.Insert(user);
        return _mapper.Map<UserDto>(newUser);
    }

    public UserDto GetUserByEmail(string email)
    {
        var user = _userRepository.GetUserByEmail(email);
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

    private void ValidateRole(UserDto userDto)
    {
        if (!_roleRepository.Exists(userDto.RoleId))
        {
            throw new InvalidRoleException("Invalid role ID.");
        }
    }
}