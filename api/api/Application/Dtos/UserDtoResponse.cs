using framework.Application.Dtos;

namespace api.Application.Dtos;

public class UserDtoResponse: IDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}