namespace Netdemo.Application.Abstractions;

public interface IJwtTokenGenerator
{
    string Generate(string userId, string email, IEnumerable<string> roles);
}
