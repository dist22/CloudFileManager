using Cloud.Data;
using Cloud.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly DataContextEF _entity;
    protected readonly DbSet<T> _dbSet;
    
    public BaseRepository(DataContextEF entity)
    {
        _entity = entity;
        _dbSet = entity.Set<T>();
    }
    
    public async Task<bool> SaveChangesAsync()
    {
        return await _entity.SaveChangesAsync() > 0;
    }

    public async Task<bool> Update<U>(U _object) where U : class
    {
        _entity.Set<U>().Update(_object);
        return await SaveChangesAsync();
    }

    public async Task<bool> Remove(T _object)
    {
        _dbSet.Remove(_object);
        return await SaveChangesAsync();
    }
}