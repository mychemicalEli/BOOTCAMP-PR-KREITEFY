using api.Application.Dtos;

namespace api.Application.Services.Interfaces;

public interface ICurrentUserService
{
    UserDtoResponse GetUserFromToken();
}