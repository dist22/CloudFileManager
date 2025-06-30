using Cloud.Domain.Interfaces.PasswordHasher;

namespace Cloud.Infrastructure.PasswordHasher;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password)
        => BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    public bool Verify(string password, string passwordConfirm)
        => BCrypt.Net.BCrypt.EnhancedVerify(password, passwordConfirm);
}