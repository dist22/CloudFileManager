using Cloud.Models;

namespace Cloud.Interfaces;
public interface IJwtProvider
{
    public string CreateToken(User user);
}