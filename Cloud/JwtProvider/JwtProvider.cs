using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cloud.Interfaces;
using Cloud.Jwt;
using Cloud.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cloud.JwtProvider;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    public string CreateToken(User user)
    {
        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()),
            new Claim(ClaimTypes.Role, user.role)
        ];

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.TokenKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
        
    }
}