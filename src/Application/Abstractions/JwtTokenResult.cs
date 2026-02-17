namespace Netdemo.Application.Abstractions;

public sealed record JwtTokenResult(string AccessToken, DateTimeOffset ExpiresAtUtc);
