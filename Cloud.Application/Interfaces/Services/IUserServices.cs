using Cloud.Application.DTOs.User;

namespace Cloud.Application.Interfaces.Services;

public interface IUserServices
{
    public Task<IEnumerable<UserDTOs>> GetUsersAsync();
    public Task<UserDTOs> GetUserAsync(int userId);
    public Task EditUserAsync(int userId, string role);
    public Task DeleteUserAsync(int userId);
    
}