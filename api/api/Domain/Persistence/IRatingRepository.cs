using api.Domain.Entities;
using framework.Domain.Persistence;

namespace api.Domain.Persistence;

public interface IRatingRepository : IGenericRepository<Rating>
{
    Task<IEnumerable<Rating>> GetAllAsync();
    Task InsertAsync(Rating rating);
}