namespace Cloud.Jwt;
public class JwtOptions
{
    public string TokenKey { get; set; } = string.Empty;
    public int ExpiresHours { get; set; }
}