using Cloud.DTOs;

namespace Cloud.Interfaces;

public interface IAuthServices
{
    public Task<bool> RegisteredAsync(UserCreateDTO userCreateDto);
    public Task<bool> LoginAsync(UserLoginDTO userLoginDto);
    protected Task<bool> PasswordConfirmAsync(string password, string passwordConfirm);
    protected Task<bool> IfExistAsync(string email, string username);
}