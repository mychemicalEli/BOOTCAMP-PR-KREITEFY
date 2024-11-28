namespace framework.Application.Services;

public interface IGenericService<D> where D : class
{
    List<D> GetAll();
    D Get(long id);
    D Insert(D dto);
    D Update(D dto);
    void Delete(long id);
}