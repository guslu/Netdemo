using Netdemo.Domain.Common;

namespace Netdemo.Domain.Entities;

public sealed class Organization : BaseEntity
{
    public string Name { get; private set; }
    public ICollection<Project> Projects { get; private set; } = new List<Project>();

    public Organization(string name)
    {
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Organization name is required.") : name.Trim();
    }
}
