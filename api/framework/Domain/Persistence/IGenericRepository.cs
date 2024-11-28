namespace framework.Domain.Persistence;

public interface IGenericRepository<T> where T : class
{
    List<T> GetAll();
    T GetById(long id);
    T Insert(T entity);
    T Update(T entity);
    void Delete(long id);
}