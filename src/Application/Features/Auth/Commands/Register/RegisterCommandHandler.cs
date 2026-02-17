using MediatR;
using Netdemo.Application.Abstractions;
using Netdemo.Application.Common.Exceptions;

namespace Netdemo.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandHandler(IIdentityService identityService, ICurrentUserService currentUserService)
    : IRequestHandler<RegisterCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (!currentUserService.IsInRole("Admin"))
        {
            throw new ForbiddenException("Only administrators can create users.");
        }

        if (currentUserService.OrganizationId != request.OrganizationId)
        {
            throw new ForbiddenException("Cross-organization access is not allowed.");
        }

        var result = await identityService.RegisterAsync(request.Email, request.Password, request.OrganizationId, request.Roles, cancellationToken);
        if (!result.Succeeded || result.UserId is null)
        {
            throw new ConflictException(string.Join("; ", result.Errors));
        }

        return new RegisterResult(result.UserId.Value);
    }
}
