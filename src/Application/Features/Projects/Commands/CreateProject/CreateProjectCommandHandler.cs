using MediatR;
using Microsoft.EntityFrameworkCore;
using Netdemo.Application.Abstractions;
using Netdemo.Application.Common.Exceptions;
using Netdemo.Domain.Entities;

namespace Netdemo.Application.Features.Projects.Commands.CreateProject;

public sealed class CreateProjectCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IAuditLogService auditLogService)
    : IRequestHandler<CreateProjectCommand, Guid>
{
    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        if (request.OrganizationId != currentUserService.OrganizationId)
        {
            throw new ForbiddenException("Cross-organization access is not allowed.");
        }

        var organizationExists = await dbContext.Organizations.AsNoTracking().AnyAsync(x => x.Id == request.OrganizationId, cancellationToken);
        if (!organizationExists)
        {
            throw new NotFoundException("Organization not found.");
        }

        var project = new Project(request.OrganizationId, request.Name, request.Description);
        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync(cancellationToken);

        await auditLogService.WriteAsync(currentUserService.OrganizationId, "ProjectCreated", $"Project '{project.Name}' created", cancellationToken);
        return project.Id;
    }
}
