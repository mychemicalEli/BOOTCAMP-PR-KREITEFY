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

        public async Task IncrementStreamsAsync(long userId, long songId)
        {
            var song = await _songRepository.GetByIdAsync(songId);

            if (song == null)
            {
                throw new Exception("Song not found");
            }

            song.Streams += 1;
            await _songRepository.UpdateAsync(song);

            var userSong = await _userSongsRepository.GetByUserIdAndSongIdAsync(userId, songId);
            if (userSong != null)
            {
                userSong.TotalStreams += 1;
                userSong.LastPlayedAt = DateTime.Now;
                await _userSongsRepository.UpdateAsync(userSong);
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
                await _userSongsRepository.InsertAsync(newUserSong);
            }
        }

        public async Task<IEnumerable<SongsForYouDto>> GetSongsForYouAsync(long userId)
        {
            return await _userSongsRepository.GetSongsForYouAsync(userId);
        }

        public async Task<PagedList<HistorySongsDto>> GetHistorySongsAsync(long userId, PaginationParameters paginationParameters)
        {
            var userSongsHistory = await _userSongsRepository.GetHistorySongsAsync(userId, paginationParameters);

            foreach (var song in userSongsHistory)
            {
                song.HumanizedPlayedAt = _dateHumanizer.HumanizeDate(song.LastPlayedAt);
            }

            return userSongsHistory;
        }
    }
}