using Netdemo.Domain.Common;

namespace Netdemo.Domain.Entities;

public sealed class Project : BaseEntity
{
    public Guid OrganizationId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public ICollection<WorkItem> Tasks { get; private set; } = new List<WorkItem>();

    public Project(Guid organizationId, string name, string? description)
    {
        OrganizationId = organizationId;
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Project name is required.") : name.Trim();
        Description = description?.Trim();
    }
}
