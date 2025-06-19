namespace Cloud.DTOs;

public class UserCreateDTO
{
    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;

    public string password { get; set; } = string.Empty;

    public string passwordConfirm { get; set; } = string.Empty;
}