namespace Netdemo.Application.Abstractions;

public interface ICurrentUserService
{
    Guid UserId { get; }
    Guid OrganizationId { get; }
    bool IsInRole(string role);
}
