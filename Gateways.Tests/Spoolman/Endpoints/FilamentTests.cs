using FluentAssertions;
using RichardSzalay.MockHttp;
using System.Text.Json;

namespace Gateways.Tests
{
    internal class FilamentTests : EndpointTest<FilamentSpoolManEndpoint>
    {
        private Vendor vendor = new Vendor() { Id = 1, Name = "Vendor1" };
        private Filament filament = new Filament() { Id = 2, Name = "Vendor1", Material = "PLA", ColorHex = "FFFFFF" };

        [Test]
        public async Task GivenNonExistingFilament_WhenGetOrCreate_CreatedFilamentldBeReturned()
        {
            // Arrange & Act
            var result = await Endpoint.GetOrCreate(vendor, "FFFF00", "PLA");

            // Assert   
            result.Should().NotBeNull();
            result.Material.Should().Be("PLA");
            result.ColorHex.Should().Be("FFFF00");
        }

        [Test]
        public async Task GivenExistingFilament_WhenGetOrCreate_ExistingFilamentShouldBeReturned()
        {
            // Arrange & Act
            var result = await Endpoint.GetOrCreate(vendor, "FFFFFF", "PLA");

            // Assert   
            result.Should().NotBeNull();
            result.Vendor.Name.Should().Be("Vendor1");
            result.Material.Should().Be("PLA");
            result.ColorHex.Should().Be("FFFFFF");
        }

        public override void SetupHttpClient(MockHttpMessageHandler mockHandler)
        {
            mockHandler
               .When("/api/v1/filament")
               .WithQueryString(FilamentQueryConstants.VendorName, "Vendor1")
               .WithQueryString(FilamentQueryConstants.ColorHex, "FFFFFF")
               .WithQueryString(FilamentQueryConstants.Material, "PLA")
               .Respond("application/json", "[{\"id\":2,\"registered\":\"2025-03-15T15:18:26Z\",\"name\":\"Gray\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"PLA\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"898989\",\"extra\":{}},{\"id\":3,\"registered\":\"2025-03-15T15:18:49Z\",\"name\":\"Peru\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"PLA\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"FFFFFF\",\"extra\":{}}]");


            mockHandler
               .When("/api/v1/filament")
               .WithQueryString(FilamentQueryConstants.VendorName, "Vendor1")
               .WithQueryString(FilamentQueryConstants.ColorHex, "FFFF00")
               .WithQueryString(FilamentQueryConstants.Material, "PLA")
               .Respond("application/json", "[]");

            mockHandler
               .When("/api/v1/filament")
               .WithContent("{\"id\":0,\"name\":\"Yellow\",\"vendor_id\":1,\"material\":\"PLA\",\"price\":0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000,\"spool_weight\":0,\"extruder_temp\":0,\"bed_temp\":0,\"color_hex\":\"FFFF00\"}")
               .Respond("application/json", JsonSerializer.Serialize(filament));

            //mockHandler
            //    .When(HttpMethod.Put, "/api/v1/spool/2/use")
            //    .WithContent("{\"use_weight\":100}")
            //    .Respond("application/json", "");
        }
    }
}