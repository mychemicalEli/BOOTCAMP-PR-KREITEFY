using api.Application.Dtos;
using framework.Application.Services;

namespace api.Application.Services.Interfaces;

public interface IRatingService : IGenericService<RatingDto>
{
    void AddRating(RatingDto ratingDto);
}