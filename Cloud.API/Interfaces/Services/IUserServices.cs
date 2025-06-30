using Cloud.DTOs;
using Cloud.DTOs.User;

namespace Cloud.Interfaces.Services;

public interface IUserServices
{
    public Task<IEnumerable<UserDTOs>> GetUsersAsync();
    public Task<UserDTOs> GetUserAsync(int userId);
    public Task EditUserAsync(int userId, string role);
    public Task DeleteUserAsync(int userId);
    
}