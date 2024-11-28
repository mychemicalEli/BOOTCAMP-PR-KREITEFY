using framework.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace framework.Infrastructure.Persistence;

public class UnitOfWork:IUnitOfWork
{
    private readonly DbContext _dbContext;

    public UnitOfWork(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IWork Init()
    {
        return new Work(_dbContext.Database.BeginTransaction());
    }

}