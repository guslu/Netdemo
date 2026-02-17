using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Netdemo.Application.Abstractions;
using Netdemo.Infrastructure.Configuration;

namespace Netdemo.Infrastructure.Services;

public sealed class JwtTokenGenerator(IOptions<JwtOptions> options) : IJwtTokenGenerator
{
    public JwtTokenResult Generate(string userId, string email, IEnumerable<string> roles, IReadOnlyDictionary<string, string>? customClaims = null)
    {
        var jwtOptions = options.Value;
        var expires = DateTimeOffset.UtcNow.AddMinutes(jwtOptions.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email),
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, email)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        if (customClaims is not null)
        {
            claims.AddRange(customClaims.Select(claim => new Claim(claim.Key, claim.Value)));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: expires.UtcDateTime,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        return new JwtTokenResult(accessToken, expires);
    }
}
