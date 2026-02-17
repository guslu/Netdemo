namespace Netdemo.Application.Abstractions;

public interface IAuditLogService
{
    Task WriteAsync(Guid organizationId, string eventType, string details, CancellationToken cancellationToken = default);
}
