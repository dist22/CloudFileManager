using System.Linq.Expressions;
using Cloud.Domain.Interfaces.Repositories;
using Cloud.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace Cloud.Infrastructure.Repository;

public class BaseRepository<T>(DataContextEF entity) : IBaseRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet = entity.Set<T>();

    public async Task<bool> IfExistAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(expression);
    }

    private async Task<bool> SaveChangesAsync()
    {
        return await entity.SaveChangesAsync() > 0;
    }

    public async Task<bool> Update(T @object)
    {
        _dbSet.Update(@object);
        return await SaveChangesAsync();
    }

    public async Task<bool> Remove(T @object)
    {
        _dbSet.Remove(@object);
        return await SaveChangesAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(expression);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<T?> AddAsync(T @object)
    {
        var result = _dbSet.Add(@object);
        return await SaveChangesAsync() ? result.Entity : null;
    }
}