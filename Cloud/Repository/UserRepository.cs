using System.Linq.Expressions;
using Cloud.Data;
using Cloud.Interfaces.Repositoryes;
using Cloud.Models;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Repository;

public class UserRepository(DataContextEF entity) : BaseRepository<User>(entity), IUserRepository
{
    public async Task<IEnumerable<User>> GetUsers() 
        => await _dbSet
            .AsNoTracking()
            .ToListAsync();

    public async Task<User?> GetUser(Expression<Func<User, bool>> expression) 
        => await _dbSet
            .AsNoTracking()
            .Include(u => u.files)
            .FirstOrDefaultAsync(expression);

    public async Task<User> GetUserIfExistAsync(Expression<Func<User, bool>> expression)
    {
        var user = await GetUser(expression);
        if(user == null)
            throw new UnauthorizedAccessException("User not found");
        return user;
    }

    public async Task<bool> EditUser(User user) 
        => await Update(user);

    public async Task<User> AddUserAsync(User user)
    {
        var result = await _dbSet.AddAsync(user);
        return await SaveChangesAsync() ? result.Entity : null;
    }

    public async Task<bool> UserExists(Expression<Func<User, bool>> predicate) 
        => await _dbSet
            .AsNoTracking()
            .AnyAsync(predicate);

    
    public async Task<bool> DeleteUser(User user) 
        => await Remove(user);
    
}