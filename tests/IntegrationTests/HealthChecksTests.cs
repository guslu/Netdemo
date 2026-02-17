using FluentAssertions;

namespace Netdemo.IntegrationTests;

public sealed class HealthChecksTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
    [Fact]
    public async Task Live_Healthcheck_Should_Return_Success()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/health/live");
        response.IsSuccessStatusCode.Should().BeTrue();
    }
}
