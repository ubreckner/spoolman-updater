using System.Net.Http.Json;

namespace Gateways;

internal class VendorSpoolManEndoint(SpoolmanConfiguration configuration) : SpoolmanEndoint<Vendor>(configuration), IVendorEndpoint
{
    // Get or create a vendor
    public async Task<Vendor> GetOrCreate(string name)
    {
        var vendorResponse = await HttpClient.GetFromJsonAsync<List<Vendor>>($"vendor?name={name}", JsonOptions);

        Vendor? vendor;
        if (vendorResponse != null && vendorResponse.Any())
            vendor = vendorResponse.First();
        else
        {
            var newVendor = new Vendor
            {
                Name = name
            };

            var createVendorResponse = await HttpClient.PostAsJsonAsync("vendor", newVendor, JsonOptions);

            vendor = createVendorResponse.IsSuccessStatusCode ? await createVendorResponse.Content.ReadFromJsonAsync<Vendor>() : null;
        }

        return vendor ?? throw new InvalidOperationException("Failed to create or retrieve vendor.");
    }
}
