using FluentAssertions;
using Moq;
using RichardSzalay.MockHttp;
using System.Text.Json;

namespace Gateways.Tests
{
    internal class SpoolTests : EndpointTest<SpoolSpoolmanEndpoint>
    {
        private Vendor vendor = new Vendor() { Id = 1, Name = "Vendor1" };
        private Filament filament = new Filament() { Id = 2, Name = "Vendor1", Material = "PLA", ColorHex = "FFFF00", Vendor = new Vendor() { Id = 1, Name = "Vendor1" } };

        [Test]
        public async Task GivenIdAndWeight_WhenUseSpoolWeight_ShouldReturnTrue()
        {
            // Arrange & Act
            var result = await Endpoint.UseSpoolWeight(2, 100);

            // Assert   
            result.Should().BeTrue();
        }

        [Test]
        public async Task GivenNonExistingSpool_WhenGetOrCreate_CreatedSpoolShouldBeReturned()
        {
            // Arrange & Act
            var result = await Endpoint.GetOrCreateSpool("Vendor1", "PLA", "#FFFF0000", string.Empty);

            // Assert   
            result.Should().NotBeNull();
            result.Filament.Vendor.Name.Should().Be("Vendor1");
            result.Filament.Material.Should().Be("PLA");
            result.Filament.ColorHex.Should().Be("FFFF00");
        }

        [Test]
        public async Task GivenExistingSpoolWithTag_WhenGetOrCreate_ExistingSpoolShouldBeReturned()
        {
            // Arrange & Act
            var result = await Endpoint.GetOrCreateSpool("Vendor1", "PLA", "#00000000", "tag1");

            // Assert   
            result.Should().NotBeNull();
            result.Filament.Vendor.Name.Should().Be("Vendor1");
            result.Filament.Material.Should().Be("Empty");
            result.Filament.ColorHex.Should().Be("000000");
            result.Extra["tag"].Should().Be("tag1");
        }

        [Test]
        public async Task GivenExistingSpool_WhenGetOrCreate_ExistingSpoolShouldBeReturned()
        {
            // Arrange & Act
            var result = await Endpoint.GetOrCreateSpool("Vendor1", "PLA", "#FFFFFFFF", string.Empty);

            // Assert   
            result.Should().NotBeNull();
            result.Filament.Vendor.Name.Should().Be("Vendor1");
            result.Filament.Material.Should().Be("PLA");
            result.Filament.ColorHex.Should().Be("FFFFFF");
        }

        public override void SetupHttpClient(MockHttpMessageHandler mockHandler)
        {
            mockHandler
               .When("/api/v1/spool")
               .WithQueryString(FilamentQueryConstants.FilamentVendorName, "Vendor1")
               .WithQueryString(FilamentQueryConstants.FilamentMaterial, "PLA")
               .Respond("application/json", "[{\"id\":1,\"registered\":\"2025-03-15T15:17:59Z\",\"first_used\":\"2025-03-15T15:17:59Z\",\"last_used\":\"2025-03-15T15:17:59Z\",\"filament\":{\"id\":1,\"registered\":\"2025-03-15T15:17:59Z\",\"name\":\"Black\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"Empty\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"000000\",\"extra\":{}},\"remaining_weight\":1000.0,\"initial_weight\":1000.0,\"spool_weight\":250.0,\"used_weight\":0.0,\"remaining_length\":335283.6194167644,\"used_length\":0.0,\"archived\":false,\"extra\":{\"tag\":\"tag1\"}},{\"id\":2,\"registered\":\"2025-03-15T15:18:38Z\",\"filament\":{\"id\":2,\"registered\":\"2025-03-15T15:18:26Z\",\"name\":\"Gray\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"PLA\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"898989\",\"extra\":{}},\"remaining_weight\":1000.0,\"initial_weight\":1000.0,\"spool_weight\":250.0,\"used_weight\":0.0,\"remaining_length\":335283.6194167644,\"used_length\":0.0,\"archived\":false,\"extra\":{}},{\"id\":3,\"registered\":\"2025-03-15T15:18:50Z\",\"filament\":{\"id\":3,\"registered\":\"2025-03-15T15:18:49Z\",\"name\":\"Peru\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"PLA\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"B87333\",\"extra\":{}},\"remaining_weight\":1000.0,\"initial_weight\":1000.0,\"spool_weight\":250.0,\"used_weight\":0.0,\"remaining_length\":335283.6194167644,\"used_length\":0.0,\"archived\":false,\"extra\":{}},{\"id\":4,\"registered\":\"2025-03-15T15:18:53Z\",\"first_used\":\"2025-03-16T10:39:58Z\",\"last_used\":\"2025-03-16T10:40:10Z\",\"filament\":{\"id\":4,\"registered\":\"2025-03-15T15:18:52Z\",\"name\":\"White\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"PLA\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"FFFFFF\",\"extra\":{}},\"remaining_weight\":976.0,\"initial_weight\":1000.0,\"spool_weight\":250.0,\"used_weight\":24.0,\"remaining_length\":327236.812550762,\"used_length\":8046.806866002346,\"archived\":false,\"extra\":{}},{\"id\":5,\"registered\":\"2025-03-15T15:18:55Z\",\"filament\":{\"id\":5,\"registered\":\"2025-03-15T15:18:54Z\",\"name\":\"Black\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"PLA\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"161616\",\"extra\":{}},\"remaining_weight\":1000.0,\"initial_weight\":1000.0,\"spool_weight\":250.0,\"used_weight\":0.0,\"remaining_length\":335283.6194167644,\"used_length\":0.0,\"archived\":false,\"extra\":{}}]");

            mockHandler
               .When("/api/v1/spool")
               .WithQueryString(FilamentQueryConstants.FilamentVendorName, "Vendor2")
               .WithQueryString(FilamentQueryConstants.FilamentMaterial, "PLA")
               .Respond("application/json", "[]");

            mockHandler
               .When("/api/v1/spool")
               .WithContent("{\"id\":0,\"filament_id\":2,\"remaining_weight\":1000,\"initial_weight\":1000,\"spool_weight\":250,\"used_length\":0,\"archived\":false,\"extra\":{}}")
               .Respond("application/json", JsonSerializer.Serialize(new Spool()
               {
                   Id = 6,
                   Filament = filament
               }));

            mockHandler
                .When(HttpMethod.Put, "/api/v1/spool/2/use")
                .WithContent("{\"use_weight\":100}")
                .Respond("application/json", "");
        }

        public override void SetupConstructorArguments()
        {
            var vendorEndpoint = new Mock<IVendorEndpoint>();
            vendorEndpoint.Setup(endpoint => endpoint.GetOrCreate(It.IsAny<string>())).Returns(Task.FromResult(vendor));

            var filamentEndpoint = new Mock<IFilamentEndpoint>();
            filamentEndpoint.Setup(endpoint => endpoint.GetOrCreate(It.IsAny<Vendor>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(filament));

            ConstructorArguments = new object[]
            {
                vendorEndpoint.Object,
                filamentEndpoint.Object,
            };
        }
    }
}