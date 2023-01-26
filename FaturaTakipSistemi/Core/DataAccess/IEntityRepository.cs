using System.Linq.Expressions;

namespace FaturaTakip.Core.DataAccess;

public interface IEntityRepository<T> where T : class, IEntity, new()
{
    Task<T> AddAsync(T entity);
    Task<T> DeleteAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> GetAsync(Expression<Func<T,bool>> filter);
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);

}