using System.Net.Http.Json;

namespace Gateways;

internal class SpoolSpoolmanEndoint : SpoolmanEndoint<Spool>, ISpoolEndpoint
{
    private readonly IVendorEndpoint vendorEndpoint;
    private readonly IFilamentEndpoint filamentEndpoint;

    public SpoolSpoolmanEndoint(
        SpoolmanConfiguration configuration,
        IVendorEndpoint vendorEndpoint,
        IFilamentEndpoint filamentEndpoint) : base(configuration)
    {
        this.vendorEndpoint = vendorEndpoint;
        this.filamentEndpoint = filamentEndpoint;
    }

    public async Task<Spool> GetOrCreateSpool(string brand, string material, string color, string tagUid)
    {
        // TODO Mapping
        if (string.IsNullOrEmpty(tagUid) || tagUid == "0000000000000000" && brand == "Bambu")
            brand = "Sunlu";

        // Fetch all spools from Spoolman
        var query = $"filament.vendor.name={brand}";

        if (!string.IsNullOrEmpty(material))
        {
            query += $"&filament.material={material}";
        }

        var allBrandSpools = await HttpClient.GetFromJsonAsync<List<Spool>>($"spool?{query}", JsonOptions);

        Spool? matchingSpool = null;
        if (allBrandSpools != null && allBrandSpools.Any())
        {
            matchingSpool = allBrandSpools.FirstOrDefault(spool => color.StartsWith($"#{spool.Filament.ColorHex}", StringComparison.OrdinalIgnoreCase) == true);
        }

        matchingSpool ??= await CreateSpoolAsync(brand, color.Substring(1, 6), material);

        return matchingSpool;
    }

    public async Task<bool> UseSpoolWeight(int spoolId, float usedWeight)
    {
        var payload = new { use_weight = usedWeight };
        var response = await HttpClient.PutAsJsonAsync($"spool/{spoolId}/use", payload);

        return response.IsSuccessStatusCode;
    }

    private async Task<Spool?> CreateSpoolAsync(string vendorName, string color, string material)
    {
        var vendor = await vendorEndpoint.GetOrCreate(vendorName);

        var filament = await filamentEndpoint.GetOrCreateFilament(vendor, color, material);

        var newSpool = new Spool
        {
            FilamentId = filament.Id,
            InitialWeight = 1000,  // Default values, adjust as needed
            RemainingWeight = 1000,
            SpoolWeight = 250
        };

        var createResponse = await HttpClient.PostAsJsonAsync($"spool", newSpool, JsonOptions);

        if (createResponse.IsSuccessStatusCode)
        {
            return await createResponse.Content.ReadFromJsonAsync<Spool>();  // Return the created spool
        }

        return null;
    }
}
