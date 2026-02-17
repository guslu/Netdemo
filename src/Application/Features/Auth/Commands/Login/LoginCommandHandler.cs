using MediatR;
using Netdemo.Application.Abstractions;
using Netdemo.Application.Common.Exceptions;

namespace Netdemo.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(IIdentityService identityService, IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await identityService.ValidateCredentialsAsync(request.Email, request.Password, cancellationToken);
        if (user is null)
        {
            throw new ForbiddenException("Invalid email or password.");
        }

        var token = jwtTokenGenerator.Generate(user.UserId.ToString(), user.Email, user.Roles);
        return new LoginResult(token.AccessToken, token.ExpiresAtUtc);
    }
}
