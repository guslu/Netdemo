using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netdemo.Application.Features.Projects.Commands.CreateProject;
using Netdemo.Application.Features.Projects.Queries.GetProjects;

namespace Netdemo.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projects")]
[Authorize]
public sealed class ProjectsController(IMediator mediator) : ControllerBase
{
    [HttpGet("organization/{organizationId:guid}")]
    [Authorize(Roles = "Admin,Manager,Member")]
    public async Task<IActionResult> GetByOrganization(Guid organizationId, CancellationToken cancellationToken)
    {
        var projects = await mediator.Send(new GetProjectsQuery(organizationId), cancellationToken);
        return Ok(projects);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetByOrganization), new { organizationId = command.OrganizationId }, new { id });
    }
}
