using api.Application.Dtos;
using api.Application.Services.Interfaces;
using api.Domain.Entities;
using api.Domain.Persistence;
using AutoMapper;
using framework.Application.Services;

namespace api.Application.Services.Implementations;

public class RatingService : GenericService<Rating, RatingDto>, IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly ISongRepository _songRepository;

    public RatingService(IRatingRepository ratingRepository, ISongRepository songRepository, IMapper mapper) : base(
        ratingRepository, mapper)
    {
        _ratingRepository = ratingRepository;
        _songRepository = songRepository;
    }
    
    public async Task AddRatingAsync(RatingDto ratingDto)
    {
        var existingRating = (await _ratingRepository.GetAllAsync())
            .FirstOrDefault(r => r.UserId == ratingDto.UserId && r.SongId == ratingDto.SongId);
        if (existingRating != null)
        {
            throw new InvalidOperationException("User already rated this song.");
        }

        var rating = new Rating
        {
            UserId = ratingDto.UserId,
            SongId = ratingDto.SongId,
            Stars = ratingDto.Stars
        };

        await _ratingRepository.InsertAsync(rating);
        await UpdateSongMediaRatingAsync(rating.SongId);
    }

    private async Task UpdateSongMediaRatingAsync(long songId)
    {
        var song = await _songRepository.GetByIdAsync(songId);
        if (song == null) return;

        var totalRatings = song.Ratings.Count;
        if (totalRatings == 0) return;

        var totalStars = song.Ratings.Sum(r => r.Stars);
        var newMediaRating = totalStars / totalRatings;

        song.MediaRating = newMediaRating;
        await _songRepository.UpdateAsync(song);
    }
    
}