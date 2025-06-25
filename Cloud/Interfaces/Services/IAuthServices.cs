using Cloud.DTOs;
using Cloud.DTOs.User;

namespace Cloud.Interfaces.Services;

public interface IAuthServices
{
    public Task<bool> RegisteredAsync(UserCreateDTO userCreateDto);
    public Task<string> LoginAsync(UserLoginDTO userLoginDto);
    protected Task<bool> PasswordConfirmAsync(string password, string passwordConfirm);
    protected Task<bool> IfExistAsync(string email, string username);
}