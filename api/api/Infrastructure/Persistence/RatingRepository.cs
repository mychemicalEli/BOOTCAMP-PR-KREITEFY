using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Infrastructure.Persistence;

namespace api.Infrastructure.Persistence;

public class RatingRepository : GenericRepository<Rating>, IRatingRepository
{
    public RatingRepository(KreitekfyContext context) : base(context)
    {
    }
}