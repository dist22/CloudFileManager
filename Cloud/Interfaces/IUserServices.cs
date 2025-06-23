using Cloud.DTOs;

namespace Cloud.Interfaces;

public interface IUserServices
{
    public Task<IEnumerable<UserDTOs>> GetUsersAsync();
    public Task<UserDTOs> GetUserAsync(int userId);
    public Task<bool> EditUserAsync(UserEditDTO userEditDto);
    public Task<bool> DeleteUserAsync(int userId);
    
}