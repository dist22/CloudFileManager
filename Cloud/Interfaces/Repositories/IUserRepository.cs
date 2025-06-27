using System.Linq.Expressions;
using Cloud.Models;

namespace Cloud.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<User> AddUserAsync(User user);
    public Task<bool> UserExists(Expression<Func<User, bool>> predicate);
    public Task<IEnumerable<User>> GetUsers();
    protected internal Task<User?> GetUser(Expression<Func<User, bool>> expression);
    public Task<User> GetUserIfExistAsync(Expression<Func<User, bool>> expression);
    public Task<bool> EditUser(User user);
    public Task<bool> DeleteUser(User user);
    
}