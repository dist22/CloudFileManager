using Cloud.DTOs;
using Cloud.DTOs.User;

namespace Cloud.Interfaces.Services;

public interface IUserServices
{
    public Task<IEnumerable<UserDTOs>> GetUsersAsync();
    public Task<UserDTOs> GetUserAsync(int userId);
    public Task<bool> EditUserAsync(int userId, string role);
    public Task<bool> DeleteUserAsync(int userId);
    
}