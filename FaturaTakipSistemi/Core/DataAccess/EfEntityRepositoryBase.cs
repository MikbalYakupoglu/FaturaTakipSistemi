using System.Linq.Expressions;
using FaturaTakip.Migrations;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.Core.DataAccess;

public class EfEntityRepositoryBase<TEntity,TContext> : IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()
    where TContext : DbContext, new()

{
    public async Task<TEntity> Add(TEntity entity)
    {
        using (TContext context = new TContext())
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }

    public async Task<TEntity> Delete(TEntity entity)
    {
        using (TContext context = new TContext())
        {
            context.Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }

    public async Task<TEntity> Update(TEntity entity)
    {
        using (TContext context = new TContext())
        {
            context.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }

    public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
    {
        using (TContext context = new TContext())
        {
            return await context.Set<TEntity>().FirstOrDefaultAsync(filter);
        }
    }

    public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null)
    {
        using (TContext context = new TContext())
        {
            return filter == null
                ? await context.Set<TEntity>().ToListAsync()
                : await context.Set<TEntity>().Where(filter).ToListAsync();
        }
    }
}