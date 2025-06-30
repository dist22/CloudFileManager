using Cloud.Domain.Models;

namespace Cloud.Domain.Interfaces.JwtProvider;
public interface IJwtProvider
{
    public string CreateToken(User user);
}