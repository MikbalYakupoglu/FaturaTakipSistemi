using FaturaTakip.Data.Models.Abstract;
using System.Linq.Expressions;

namespace FaturaTakip.DataAccess.Core
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        Task<T> GetAsync(Expression<Func<T,bool>> filter);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>> filter = null);
        Task AddAsync(T entity);
        Task RemoveAsync(T entity);
        Task UpdateAsync(T entity);
        bool IsAnyExist();
    }
}
