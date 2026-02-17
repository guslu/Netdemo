namespace Netdemo.Application.Abstractions;

public interface IJwtTokenGenerator
{
    JwtTokenResult Generate(string userId, string email, IEnumerable<string> roles);
}
