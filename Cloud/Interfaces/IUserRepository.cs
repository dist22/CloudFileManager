using Cloud.DTOs;
using Cloud.Models;


namespace Cloud.Interfaces;

public interface IUserRepository
{
    public Task CreateUser(UserForCreate userForCreate);
    public Task<bool> GetUserByEmail(string email);
    public Task<bool> GetUserByUserName(string userName);
    public Task<IEnumerable<User>> GetListOfUsers();
    public Task<User> GetUserById(int id);
}