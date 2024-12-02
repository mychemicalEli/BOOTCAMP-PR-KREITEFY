using api.Application.Dtos;
using api.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace api.Infrastructure.Rest
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [Produces("application/json")]
        public ActionResult Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingUser = _userService.GetAllUsersWithRoleName().FirstOrDefault(u => u.Email == userDto.Email);
                if (existingUser != null)
                {
                    return BadRequest("El correo electrónico ya está registrado.");
                }

                // Registra al nuevo usuario
                var newUser = _userService.RegisterUser(userDto);

                // Genera el token para el usuario registrado
                var token = GenerateJwtToken(newUser);

                // Devuelve el usuario, el token y el nombre
                return Ok(new
                {
                    User = newUser,
                    Token = token,
                    UserName = newUser.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login")]
        [Produces("application/json")]
        public ActionResult<LoginDto> Login([FromBody] LoginDto loginDto)
        {
            var user = _userService.GetAllUsersWithRoleName()
                .FirstOrDefault(u => u.Email.Equals(loginDto.Email, StringComparison.OrdinalIgnoreCase));

            if (user == null || user.Password != loginDto.Password)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = GenerateJwtToken(user);

            // Devolver el token junto con el nombre de usuario
            return Ok(new { Token = token, UserName = user.Name });
        }


        private string GenerateJwtToken(UserDto user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Name),
            };

            var key = new SymmetricSecurityKey(RandomNumberGenerator.GetBytes(32));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "your_issuer",
                audience: "your_audience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}