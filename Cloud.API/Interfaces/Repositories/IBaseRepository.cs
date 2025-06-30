using System.Linq.Expressions;

namespace Cloud.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class
{
    public Task<bool> Update(T @object);

    public Task<bool> Remove(T @object);
    
    public Task<T?> GetAsync(Expression<Func<T, bool>> expression);
    
    public Task<IEnumerable<T>> GetAllAsync();
    
    public Task<T?> AddAsync(T @object);

    public Task<bool> IfExistAsync(Expression<Func<T, bool>> expression);

    protected Task<bool> SaveChangesAsync();
    
}