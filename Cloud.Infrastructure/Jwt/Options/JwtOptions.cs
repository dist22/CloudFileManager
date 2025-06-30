namespace Cloud.Infrastructure.Jwt.Options;
public class JwtOptions
{
    public string TokenKey { get; set; } = string.Empty;
    public int ExpiresHours { get; set; }
}