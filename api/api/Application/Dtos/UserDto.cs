using framework.Application.Dtos;

namespace api.Application.Dtos;

public class UserDto : IDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required long RoleId { get; set; }
    public required string RoleName { get; set; }
}