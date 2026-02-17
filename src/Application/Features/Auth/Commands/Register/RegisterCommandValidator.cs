using FluentValidation;

namespace Netdemo.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private static readonly string[] AllowedRoles = ["Admin", "Manager", "Member"];

    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(12)
            .Matches("[A-Z]").WithMessage("Password must include at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must include at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must include at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must include at least one non-alphanumeric character.");

        RuleFor(x => x.OrganizationId).NotEqual(Guid.Empty);
        RuleFor(x => x.Roles)
            .NotEmpty()
            .Must(r => r.All(role => AllowedRoles.Contains(role)))
            .WithMessage("Roles must only contain Admin, Manager, or Member.");
    }
}
