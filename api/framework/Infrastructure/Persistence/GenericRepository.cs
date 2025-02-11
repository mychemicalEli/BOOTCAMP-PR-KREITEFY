using System.Linq.Expressions;
using System.Reflection;
using framework.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace framework.Infrastructure.Persistence;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual List<T> GetAll()
    {
        return _dbSet.ToList<T>();
    }

    public virtual T GetById(long id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null)
        {
            throw new ElementNotFoundException();
        }

        return entity;
    }

    public virtual T Insert(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public virtual T Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
        return entity;
    }

    public virtual void Delete(long id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null)
            throw new ElementNotFoundException();
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }
    
    protected virtual IQueryable<T> ApplySortOrder(IQueryable<T> entities, string sortOrder)
    {
        var orderByParameters = sortOrder.Split(',');
        
        var orderByAttribute = Char.ToUpper(orderByParameters[0][0]) + orderByParameters[0][1..];
        
        var orderByDirection = orderByParameters.Length > 1 ? orderByParameters[1] : "asc";

        var propertyInfo = typeof(T).GetProperty(orderByAttribute,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        
        if (propertyInfo != null)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyInfo);
            
            if (propertyInfo.PropertyType.IsValueType)
            {
              
                var orderByExpression =
                    Expression.Lambda<Func<T, dynamic>>(Expression.Convert(property, typeof(object)), parameter);
                
                entities = orderByDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? entities.OrderBy(orderByExpression)
                    : entities.OrderByDescending(orderByExpression);
            }
            else
            {
                var orderByExpression = Expression.Lambda<Func<T, object>>(property, parameter);
                entities = orderByDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? entities.OrderBy(orderByExpression)
                    : entities.OrderByDescending(orderByExpression);
            }
        }
        return entities;
    }
}