using api.Domain.Entities;
using api.Domain.Persistence;
using framework.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace api.Infrastructure.Persistence;

public class RatingRepository : GenericRepository<Rating>, IRatingRepository
{
    private readonly KreitekfyContext _context;

    public RatingRepository(KreitekfyContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Rating>> GetAllAsync()
    {
        return await _context.Set<Rating>().ToListAsync();
    }

    public async Task InsertAsync(Rating rating)
    {
        await _context.Set<Rating>().AddAsync(rating);
        await _context.SaveChangesAsync();
    }
}