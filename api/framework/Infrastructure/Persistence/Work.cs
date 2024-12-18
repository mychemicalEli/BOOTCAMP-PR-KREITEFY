using framework.Domain.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace framework.Infrastructure.Persistence;

public class Work : IWork
{
    private readonly IDbContextTransaction _transactionScope;
    private bool _disposed = false;

    public Work(IDbContextTransaction transactionScope)
    {
        _transactionScope = transactionScope;
    }

    public void Complete()
    {
        _transactionScope.Commit();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transactionScope.Dispose();
            }

            _disposed = true;
        }
    }

    public void Rollback()
    {
        _transactionScope.Rollback();
    }
}