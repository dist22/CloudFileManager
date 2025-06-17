using Cloud.DTOs;

namespace Cloud.Interfaces;

public interface IUserServices
{
    public Task<IEnumerable<UserDTOs>> GetUsersAsync();

    public Task<UserDTOs> GetUserAsync(int userId);

    public Task<bool> CreateUserAsync(UserForCreate userForCreate);

    public Task<bool> EditUserAsync(int userId, UserForEdit userForEdit);

    public Task<bool> DeleteUserAsync(int userId);

}