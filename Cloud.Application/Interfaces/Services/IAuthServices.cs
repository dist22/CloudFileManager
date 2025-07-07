using Cloud.Application.DTOs.User;

namespace Cloud.Application.Interfaces.Services;

public interface IAuthServices
{
    public Task RegisteredAsync(UserCreateDTO userCreateDto);
    public Task<string> LoginAsync(UserLoginDTO userLoginDto);
    
}