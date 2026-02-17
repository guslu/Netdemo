using FluentValidation;

namespace Netdemo.Application.Features.WorkItems.Commands.CreateWorkItem;

public sealed class CreateWorkItemCommandValidator : AbstractValidator<CreateWorkItemCommand>
{
    public CreateWorkItemCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEqual(Guid.Empty);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Description).MaximumLength(4000);
    }
}
