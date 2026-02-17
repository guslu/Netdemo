using MediatR;
using Netdemo.Application.Abstractions;
using Netdemo.Domain.Entities;

namespace Netdemo.Application.Features.Projects.Commands.CreateProject;

public sealed class CreateProjectCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateProjectCommand, Guid>
{
    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = new Project(request.OrganizationId, request.Name, request.Description);
        dbContext.Projects.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
