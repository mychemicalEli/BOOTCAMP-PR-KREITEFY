using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Persistence;
using AutoMapper;
using framework.Application.Services;
using System.Linq;
using System.Collections.Generic;

namespace api.Application.Services.Implementations
{
    public class UserSongsService : GenericService<UserSongs, UserSongsDto>, IUserSongsService
    {
        private IUserSongsRepository _userSongsRepository;
        private ISongRepository _songRepository;

        public UserSongsService(IUserSongsRepository userSongsRepository, ISongRepository songRepository, IMapper mapper) 
            : base(userSongsRepository, mapper)
        {
            _userSongsRepository = userSongsRepository;
            _songRepository = songRepository;
        }

        public IEnumerable<UserSongsDto> GetUserSongs(long userId)
        {
            var userSongs = _userSongsRepository.GetUserSongsWithSong(userId);
            var userSongsDto = _mapper.Map<IEnumerable<UserSongsDto>>(userSongs);
            return userSongsDto;
        }

        public void IncrementStreams(long userId, long songId)
        {
            var song = _songRepository.GetById(songId);

            if (song == null)
            {
                throw new Exception("Song not found");
            }

            song.Streams += 1;
            _songRepository.Update(song);

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
}
