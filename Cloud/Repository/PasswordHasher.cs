using Cloud.Interfaces;

namespace Cloud.Repository;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password)
        => BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    public bool Verify(string password, string passwordConfirm)
        => BCrypt.Net.BCrypt.EnhancedVerify(password, passwordConfirm);
}