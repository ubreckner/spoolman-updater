using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Gateways;

internal class SpoolSpoolmanEndpoint : SpoolmanEndpoint<Spool>, ISpoolEndpoint
{
    private readonly SpoolmanConfiguration configuration;
    private readonly IVendorEndpoint vendorEndpoint;
    private readonly IFilamentEndpoint filamentEndpoint;

    public SpoolSpoolmanEndpoint(
        SpoolmanConfiguration configuration,
        IVendorEndpoint vendorEndpoint,
        IFilamentEndpoint filamentEndpoint) : base(configuration)
    {
        this.configuration = configuration;
        this.vendorEndpoint = vendorEndpoint;
        this.filamentEndpoint = filamentEndpoint;
    }

    protected override string Endpoint => "spool";

    public async Task<Spool> GetOrCreateSpool(string vendorName, string material, string color, string tagUid)
    {
        if (Spool.IsEmptyTag(tagUid))
            vendorName = GetMappedBrandName(vendorName);

        var query = $"{FilamentQueryConstants.FilamentVendorName}={vendorName}";

        if (!string.IsNullOrEmpty(material))
        {
            query += $"&{FilamentQueryConstants.FilamentMaterial}={material}";
        }

        var allBrandSpools = await GetAllAsync(query);

        Spool? matchingSpool = null;
        if (allBrandSpools != null && allBrandSpools.Any())
        {
            var colorMatchingSpools = allBrandSpools.Where(spool => color.StartsWith($"#{spool.Filament.ColorHex}", StringComparison.OrdinalIgnoreCase) == true);

            if (!Spool.IsEmptyTag(tagUid))
            {
                var jsonEncoded = JsonSerializer.Serialize(tagUid, JsonOptions);

                matchingSpool = colorMatchingSpools.FirstOrDefault(spool => spool.Extra.ContainsKey("tag") && spool.Extra["tag"] == jsonEncoded);
            }
            else
            {
                matchingSpool = colorMatchingSpools.FirstOrDefault();
            }
        }

        matchingSpool ??= await CreateSpoolAsync(vendorName, color.Substring(1, 6), material, tagUid);

        return matchingSpool;
    }

    public async Task<bool> UseSpoolWeight(int spoolId, float usedWeight)
    {
        var payload = new { use_weight = usedWeight };
        var response = await HttpClient.PutAsJsonAsync($"{Endpoint}/{spoolId}/use", payload);

        return response.IsSuccessStatusCode;
    }

    private async Task<Spool?> CreateSpoolAsync(string vendorName, string color, string material, string tagUid)
    {
        var vendor = await vendorEndpoint.GetOrCreate(vendorName);

        var filament = await filamentEndpoint.GetOrCreate(vendor, color, material);

        var newSpool = new Spool
        {
            FilamentId = filament.Id,
            InitialWeight = 1000,  // Default values, adjust as needed
            RemainingWeight = 1000,
            SpoolWeight = 250,
            Extra = !Spool.IsEmptyTag(tagUid) ? new Dictionary<string, string>
            {
                { "tag", JsonSerializer.Serialize(tagUid, JsonOptions) }
            } : new()
        };

        return await PostAsync(newSpool);
    }

    private string GetMappedBrandName(string vendorName)
    {
        foreach (var mapping in configuration.VendorMappings.Where(mapping => Regex.IsMatch(vendorName, mapping.Pattern)))
        {
            return mapping.NewVendorName;
        }

        return vendorName;
    }
}
