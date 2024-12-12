using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Persistence;
using AutoMapper;
using framework.Application.Services;
using framework.Application;

namespace api.Application.Services.Implementations
{
    public class UserSongsService : GenericService<UserSongs, UserSongsDto>, IUserSongsService
    {
        private IUserSongsRepository _userSongsRepository;
        private ISongRepository _songRepository;
        private IDateHumanizer _dateHumanizer;

        public UserSongsService(IUserSongsRepository userSongsRepository, ISongRepository songRepository,
            IMapper mapper, IDateHumanizer dateHumanizer)
            : base(userSongsRepository, mapper)
        {
            _userSongsRepository = userSongsRepository;
            _songRepository = songRepository;
            _dateHumanizer = dateHumanizer;
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

        public IEnumerable<SongsForYouDto> GetSongsForYou(long userId)
        {
            return _userSongsRepository.GetSongsForYou(userId);
        }
        
        public PagedList<HistorySongsDto> GetHistorySongs(long userId, PaginationParameters paginationParameters)
        {
            var userSongsHistory = _userSongsRepository.GetHistorySongs(userId, paginationParameters);
            
            foreach (var song in userSongsHistory)
            {
                song.HumanizedPlayedAt = _dateHumanizer.HumanizeDate(song.LastPlayedAt);
            }

            return userSongsHistory;
        }

    }
}