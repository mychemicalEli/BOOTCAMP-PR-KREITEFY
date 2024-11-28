namespace framework.Domain.Persistence;

public interface IWork : IDisposable
{
    void Complete();
    void Rollback();
}