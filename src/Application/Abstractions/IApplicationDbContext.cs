using Microsoft.EntityFrameworkCore;
using Netdemo.Domain.Entities;

namespace Netdemo.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Project> Projects { get; }
    DbSet<Organization> Organizations { get; }
    DbSet<WorkItem> WorkItems { get; }
    DbSet<Comment> Comments { get; }
    DbSet<AuditLog> AuditLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
