using Cloud.Application.DTOs.User;

namespace Cloud.Application.Interfaces.Services;

public interface IAuthServices
{
    public Task<bool> RegisteredAsync(UserCreateDTO userCreateDto);
    public Task<string> LoginAsync(UserLoginDTO userLoginDto);
    
}