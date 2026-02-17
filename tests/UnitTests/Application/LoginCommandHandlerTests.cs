using FluentAssertions;
using Moq;
using Netdemo.Application.Abstractions;
using Netdemo.Application.Common.Exceptions;
using Netdemo.Application.Features.Auth.Commands.Login;

namespace Netdemo.UnitTests.Application;

public sealed class LoginCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Token_When_Credentials_Are_Valid()
    {
        var identityService = new Mock<IIdentityService>();
        var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();

        var user = new IdentityUserProfile(Guid.NewGuid(), "member@netdemo.local", Guid.NewGuid(), new[] { "Member" });
        identityService
            .Setup(x => x.ValidateCredentialsAsync("member@netdemo.local", "SecurePassword!123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        jwtTokenGenerator
            .Setup(x => x.Generate(user.UserId.ToString(), user.Email, user.Roles, It.IsAny<IReadOnlyDictionary<string, string>>()))
            .Returns(new JwtTokenResult("jwt-token", DateTimeOffset.UtcNow.AddMinutes(60)));

        var handler = new LoginCommandHandler(identityService.Object, jwtTokenGenerator.Object);

        var result = await handler.Handle(new LoginCommand("member@netdemo.local", "SecurePassword!123"), CancellationToken.None);

        result.AccessToken.Should().Be("jwt-token");
        result.OrganizationId.Should().Be(user.OrganizationId);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Credentials_Are_Invalid()
    {
        var identityService = new Mock<IIdentityService>();
        var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();

        identityService
            .Setup(x => x.ValidateCredentialsAsync("member@netdemo.local", "wrong", It.IsAny<CancellationToken>()))
            .ReturnsAsync((IdentityUserProfile?)null);

        var handler = new LoginCommandHandler(identityService.Object, jwtTokenGenerator.Object);

        Func<Task> act = async () => await handler.Handle(new LoginCommand("member@netdemo.local", "wrong"), CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenException>();
    }
}
