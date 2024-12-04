using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Persistence;
using AutoMapper;
using framework.Application.Services;

namespace api.Application.Services.Implementations;

public class UserSongsService : GenericService<UserSongs, UserSongsDto>, IUserSongsService
{
    private IUserSongsRepository _userSongsRepository;

    public UserSongsService(IUserSongsRepository userSongsRepository, IMapper mapper) : base(userSongsRepository,
        mapper)
    {
        _userSongsRepository = userSongsRepository;
    }
    
    public void IncrementStreams(long userId, long songId)
    {
        var userSong = _userSongsRepository.GetByUserIdAndSongId(userId, songId);
        if (userSong != null)
        {
            userSong.TotalStreams += 1;
            userSong.LastPlayedAt = DateTime.Now;
            _userSongsRepository.Update(userSong);
        }
        else
        {
            var newUserSong = new UserSongs
            {
                UserId = userId,
                SongId = songId,
                TotalStreams = 1,
                LastPlayedAt = DateTime.Now
            };
            _userSongsRepository.Insert(newUserSong);
        }
    }
}