using System.Linq.Expressions;
using Cloud.DTOs;
using Cloud.Models;

namespace Cloud.Interfaces;

public interface IUserRepository
{
    public Task CreateUser(UserForCreate userForCreate);
    public Task<bool> UserExists(Expression<Func<User, bool>> predicate);
    public Task<IEnumerable<UserDTOs>> GetListOfUsers();
    public Task<T> GetUserById<T>(int id);
    public Task EditUser(User user, UserForEdit userForEdit);
    public Task DeleteUser(User user);
    public Task AddFileToUserAsync(User user, FileRecord fileRecord);
    public Task<bool> DeleteFileFromUserASync(User user,FileRecord fileRecord);
    public Task<bool> SaveChangesAsync(User user);
    
}