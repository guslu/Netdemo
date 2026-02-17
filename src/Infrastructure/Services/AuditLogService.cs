using Netdemo.Application.Abstractions;
using Netdemo.Domain.Entities;
using Netdemo.Infrastructure.Data;

namespace Netdemo.Infrastructure.Services;

public sealed class AuditLogService(ApplicationDbContext dbContext) : IAuditLogService
{
    public async Task WriteAsync(Guid organizationId, string eventType, string details, CancellationToken cancellationToken = default)
    {
        dbContext.AuditLogs.Add(new AuditLog(organizationId, eventType, details));
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
