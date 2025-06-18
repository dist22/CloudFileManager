using System.Linq.Expressions;
using Cloud.Data;
using Cloud.Interfaces;
using Cloud.Models;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Repository;

public class UserRepository(DataContextEF entity) : BaseRepository<User>(entity), IUserRepository
{
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _dbSet
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User> GetUser(int id)
    {
        return await _dbSet
                            .AsNoTracking()
                            .Include(u => u.files)
                            .FirstOrDefaultAsync(u => u.userId == id) ??
                        throw new Exception("Error");
    }

    public async Task<bool> EditUser(User user)
    {
        return await Update(user);
    }

    public async Task<User> AddUserAsync(User user)
    {
        var result = await _dbSet.AddAsync(user);
        return await SaveChangesAsync() ? result.Entity : null;
    }

    public async Task<bool> UserExists(Expression<Func<User, bool>> predicate)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(predicate);
    }
    
    public async Task<bool> DeleteUser(User user)
    {
        return await Remove(user);
    }
}