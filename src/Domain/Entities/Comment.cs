using Netdemo.Domain.Common;

namespace Netdemo.Domain.Entities;

public sealed class Comment : BaseEntity
{
    public Guid WorkItemId { get; private set; }
    public Guid AuthorId { get; private set; }
    public string Content { get; private set; }

    public Comment(Guid workItemId, Guid authorId, string content)
    {
        WorkItemId = workItemId;
        AuthorId = authorId;
        Content = string.IsNullOrWhiteSpace(content) ? throw new ArgumentException("Comment content is required.") : content.Trim();
    }
}
