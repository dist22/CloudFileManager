using Cloud.DTOs;
using Cloud.DTOs.User;

namespace Cloud.Interfaces.Services;

public interface IAuthServices
{
    public Task<bool> RegisteredAsync(UserCreateDTO userCreateDto);
    public Task<string> LoginAsync(UserLoginDTO userLoginDto);
    
}