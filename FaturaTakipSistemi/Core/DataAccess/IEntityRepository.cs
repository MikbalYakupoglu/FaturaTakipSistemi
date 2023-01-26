using System.Linq.Expressions;

namespace FaturaTakip.Core.DataAccess;

public interface IEntityRepository<T> where T : class, IEntity, new()
{
    Task<T> Add(T entity);
    Task<T> Delete(T entity);
    Task<T> Update(T entity);
    Task<T> Get(Expression<Func<T,bool>> filter);
    Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null);

}