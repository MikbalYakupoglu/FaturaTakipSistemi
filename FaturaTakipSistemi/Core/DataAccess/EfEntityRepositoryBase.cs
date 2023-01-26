using System.Linq.Expressions;
using FaturaTakip.Data;
using FaturaTakip.Migrations;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.Core.DataAccess;

public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()

{
    private readonly InvoiceTrackContext _context;
    public EfEntityRepositoryBase(InvoiceTrackContext context)
    {
        _context = context;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {

            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {

            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
    {

            return await _context.Set<TEntity>().FirstOrDefaultAsync(filter);
        
    }

    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null)
    {

            return filter == null
                ? await _context.Set<TEntity>().ToListAsync()
                : await _context.Set<TEntity>().Where(filter).ToListAsync();
        
    }
}