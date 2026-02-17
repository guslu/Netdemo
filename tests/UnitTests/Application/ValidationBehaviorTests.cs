using FluentAssertions;
using FluentValidation;
using MediatR;
using Netdemo.Application.Behaviors;

namespace Netdemo.UnitTests.Application;

public sealed class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_Should_Invoke_Next_When_There_Are_No_Validators()
    {
        var behavior = new ValidationBehavior<TestRequest, string>(Array.Empty<IValidator<TestRequest>>());
        RequestHandlerDelegate<string> next = () => Task.FromResult("ok");

        var result = await behavior.Handle(new TestRequest("value"), next, CancellationToken.None);

        result.Should().Be("ok");
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Validation_Fails()
    {
        var validators = new IValidator<TestRequest>[] { new TestRequestValidator() };
        var behavior = new ValidationBehavior<TestRequest, string>(validators);
        RequestHandlerDelegate<string> next = () => Task.FromResult("ok");

        Func<Task> act = async () => await behavior.Handle(new TestRequest(string.Empty), next, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    private sealed record TestRequest(string Name);

    private sealed class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
