using Netdemo.Domain.Common;

namespace Netdemo.Domain.Entities;

public sealed class WorkItem : BaseEntity
{
    public Guid ProjectId { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();

    public WorkItem(Guid projectId, string title, string? description)
    {
        ProjectId = projectId;
        Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Task title is required.") : title.Trim();
        Description = description?.Trim();
    }
}
