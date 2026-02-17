using FluentAssertions;
using Netdemo.Application.Features.Projects.Commands.CreateProject;

namespace Netdemo.UnitTests.Application;

public sealed class CreateProjectCommandValidatorTests
{
    [Fact]
    public void Validator_Should_Fail_When_Name_Is_Empty()
    {
        var validator = new CreateProjectCommandValidator();
        var result = validator.Validate(new CreateProjectCommand(Guid.NewGuid(), string.Empty, null));
        result.IsValid.Should().BeFalse();
    }
}
