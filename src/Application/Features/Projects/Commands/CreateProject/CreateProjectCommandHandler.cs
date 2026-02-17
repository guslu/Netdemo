using MediatR;
using Microsoft.EntityFrameworkCore;
using Netdemo.Application.Abstractions;
using Netdemo.Application.Common.Exceptions;
using Netdemo.Domain.Entities;

namespace Netdemo.Application.Features.Projects.Commands.CreateProject;

public sealed class CreateProjectCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateProjectCommand, Guid>
{
    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var organizationExists = await dbContext.Organizations
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.OrganizationId, cancellationToken);

        if (!organizationExists)
        {
            throw new NotFoundException("Organization was not found.");
        }

        var entity = new Project(request.OrganizationId, request.Name, request.Description);
        dbContext.Projects.Add(entity);

        dbContext.AuditLogs.Add(new AuditLog(
            request.OrganizationId,
            "ProjectCreated",
            $"Project '{entity.Name}' created with id '{entity.Id}'."));

        await dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
