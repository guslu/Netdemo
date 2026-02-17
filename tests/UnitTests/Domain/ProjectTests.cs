using FluentAssertions;
using Netdemo.Domain.Entities;

namespace Netdemo.UnitTests.Domain;

public sealed class ProjectTests
{
    [Fact]
    public void Constructor_Should_Trim_Name()
    {
        var project = new Project(Guid.NewGuid(), "  Platform Revamp  ", "desc");
        project.Name.Should().Be("Platform Revamp");
    }
}
