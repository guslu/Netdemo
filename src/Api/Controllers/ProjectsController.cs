using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netdemo.Application.Features.Comments.Commands.CreateComment;
using Netdemo.Application.Features.Projects.Commands.CreateProject;
using Netdemo.Application.Features.Projects.Queries.GetProjects;
using Netdemo.Application.Features.WorkItems.Commands.CreateWorkItem;
using Netdemo.Application.Features.WorkItems.Commands.UpdateWorkItem;
using Netdemo.Application.Features.WorkItems.Queries.GetByProject;

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

    [HttpGet("{projectId:guid}/work-items")]
    [Authorize(Roles = "Admin,Manager,Member")]
    public async Task<IActionResult> GetWorkItems(Guid projectId, CancellationToken cancellationToken)
    {
        var items = await mediator.Send(new GetWorkItemsByProjectQuery(projectId), cancellationToken);
        return Ok(items);
    }

    [HttpPost("work-items")]
    [Authorize(Roles = "Admin,Manager,Member")]
    public async Task<IActionResult> CreateWorkItem(CreateWorkItemCommand command, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return Ok(new { id });
    }

    [HttpPut("work-items/{id:guid}")]
    [Authorize(Roles = "Admin,Manager,Member")]
    public async Task<IActionResult> UpdateWorkItem(Guid id, UpdateWorkItemCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("comments")]
    [Authorize(Roles = "Admin,Manager,Member")]
    public async Task<IActionResult> CreateComment(CreateCommentCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
