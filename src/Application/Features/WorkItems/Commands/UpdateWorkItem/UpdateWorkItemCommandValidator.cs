using FluentValidation;

namespace Netdemo.Application.Features.WorkItems.Commands.UpdateWorkItem;

public sealed class UpdateWorkItemCommandValidator : AbstractValidator<UpdateWorkItemCommand>
{
    public UpdateWorkItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Description).MaximumLength(4000);
    }
}
