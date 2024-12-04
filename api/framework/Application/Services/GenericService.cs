using AutoMapper;
using framework.Domain.Persistence;
namespace framework.Application.Services;

public class GenericService<E, D> : IGenericService<D>
    where E : class
    where D : class
{
    protected readonly IGenericRepository<E> SongRepository;
    protected readonly IMapper _mapper;


    public GenericService(IGenericRepository<E> songRepository, IMapper mapper)
    {
        SongRepository = songRepository;
        _mapper = mapper;
    }

    public virtual List<D> GetAll()
    {
        var entity = SongRepository.GetAll();
        var dto = _mapper.Map<List<D>>(entity);
        return dto;
    }

    public virtual D Get(long id)
    {
        var entity = SongRepository.GetById(id);
        return _mapper.Map<D>(entity);
    }

    public virtual D Insert(D dto)
    {
        E entity = _mapper.Map<E>(dto);
        entity = SongRepository.Insert(entity);
        return _mapper.Map<D>(entity);
    }

    public virtual D Update(D dto)
    {
        E entity = _mapper.Map<E>(dto);
        entity = SongRepository.Update(entity);
        return _mapper.Map<D>(entity);
    }

    public virtual void Delete(long id)
    {
        SongRepository.Delete(id);
    }
}