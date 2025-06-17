namespace Cloud.Interfaces;

public interface IBaseRepository<T> where T : class
{
    public Task<bool> SaveChangesAsync();
    
    public Task<bool> Update<U>(U _object) where U : class;

    public Task<bool> Remove(T _object);
}