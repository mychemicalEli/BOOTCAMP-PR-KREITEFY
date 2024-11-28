namespace framework.Domain.Persistence;

public interface IUnitOfWork
{
    IWork Init();
}