using System.Linq.Expressions;
using Cloud.Models;

namespace Cloud.Interfaces;

public interface IUserRepository
{
    public Task<User> AddUserAsync(User user);
    public Task<bool> UserExists(Expression<Func<User, bool>> predicate);
    public Task<IEnumerable<User>> GetUsers();
    public Task<User> GetUser(int id);
    public Task<bool> EditUser(User user);
    public Task<bool> DeleteUser(User user);
    
}