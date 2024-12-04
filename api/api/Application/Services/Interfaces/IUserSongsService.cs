using api.Application.Dtos;
using framework.Application.Services;

namespace api.Application.Services.Interfaces;

public interface IUserSongsService: IGenericService<UserSongsDto>
{
    void IncrementStreams(long userId, long songId);
}