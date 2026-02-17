using Netdemo.Domain.Common;

namespace Netdemo.Domain.Entities;

public sealed class AuditLog : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string EventType { get; private set; }
    public string Details { get; private set; }

    public AuditLog(Guid organizationId, string eventType, string details)
    {
        OrganizationId = organizationId;
        EventType = eventType;
        Details = details;
    }
}
