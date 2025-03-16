using FluentAssertions;
using RichardSzalay.MockHttp;

namespace Gateways.Tests
{
    internal class VendorTests : EndpointTest<VendorSpoolmanEndpoint>
    {
        [Test]
        public async Task GivenNonExistingVendorName_WhenGetOrCreate_CreatedVendorShouldBeReturned()
        {
            // Arrange & Act
            var result = await Endpoint.GetOrCreate("Vendor2");

            // Assert   
            result.Should().NotBeNull();
            result.Name.Should().Be("Vendor2");
        }

        [Test]
        public async Task GivenExistingVendorName_WhenGetOrCreate_ExistingVendorShouldBeReturned()
        {
            // Arrang e& Act
            var result = await Endpoint.GetOrCreate("Vendor1");

            // Assert   
            result.Should().NotBeNull();
            result.Name.Should().Be("Vendor1");
        }

        public override void SetupHttpClient(MockHttpMessageHandler mockHandler)
        {
            mockHandler
               .When("/api/v1/vendor")
               .WithQueryString("name", "Vendor1")
               .Respond("application/json", "[{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor\",\"extra\":{}}]");

            mockHandler
               .When("/api/v1/vendor")
               .WithQueryString("name", "Vendor2")
               .Respond("application/json", "[]");

            mockHandler.When("/api/v1/vendor")
                .WithContent("{\"name\":\"Vendor2\"}")
                .Respond("application/json", "{\"id\":2,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor2\",\"external_id\":\"Vendor2\",\"extra\":{}}");
        }
    }
}