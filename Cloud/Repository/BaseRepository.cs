using System.Linq.Expressions;
using Cloud.Data;
using Cloud.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly DataContextEF _entity;
    protected readonly DbSet<T> dbSet;

    public BaseRepository(DataContextEF entity)
    {
        _entity = entity;
        dbSet = entity.Set<T>();
    }

    public async Task<bool> IfExistAsync(Expression<Func<T, bool>> expression)
    {
        return await dbSet
            .AsNoTracking()
            .AnyAsync(expression);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _entity.SaveChangesAsync() > 0;
    }

    public async Task<bool> Update(T @object)
    {
        dbSet.Update(@object);
        return await SaveChangesAsync();
    }

    public async Task<bool> Remove(T @object)
    {
        dbSet.Remove(@object);
        return await SaveChangesAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression)
    {
        return await dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(expression);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await dbSet
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<T?> AddAsync(T @object)
    {
        var result = dbSet.Add(@object);
        return await SaveChangesAsync() ? result.Entity : null;
    }
}