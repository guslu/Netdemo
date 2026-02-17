using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Netdemo.IntegrationTests;

public sealed class HealthChecksTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Live_Healthcheck_Should_Return_Success()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/health/live");
        response.IsSuccessStatusCode.Should().BeTrue();
    }
}
