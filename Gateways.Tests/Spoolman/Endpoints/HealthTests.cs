using FluentAssertions;
using RichardSzalay.MockHttp;

namespace Gateways.Tests
{
    internal class HealthTests : EndpointTest<HealthCheckSpoolmanEndpoint>
    {
        [Test]
        public async Task WhenCheckHealthAsync_TrueShouldBeReturned()
        {
            // Arrange & Act
            var result = await Endpoint.CheckHealthAsync();

            // Assert   
            result.Should().BeTrue();
        }

        public override void SetupHttpClient(MockHttpMessageHandler mockHandler)
        {
            mockHandler
               .When("/api/v1/health")
               .Respond("application/json", "");
        }
    }
}