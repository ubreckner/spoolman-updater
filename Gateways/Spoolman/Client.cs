using System.Net.Http.Json;
using System.Text.Json;

namespace Gateways;

public class SpoolmanClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public SpoolmanClient(SpoolmanConfiguration configuration)
    {
        _baseUrl = configuration.Url;

        _httpClient = new HttpClient();
    }

    public async Task<bool> UseSpoolWeightAsync(int spoolId, float usedWeight)
    {
        var payload = new { use_weight = usedWeight };
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/v1/spool/{spoolId}/use", payload);

        return response.IsSuccessStatusCode;
    }

    public async Task<Spool?> GetSpoolByBrandAndColorAsync(string brand, string material, string color)
    {
        try
        {
            // TODO Mapping
            if (brand == "Bambu")
                brand = "Sunlu";

            // Fetch all spools from Spoolman
            var query = $"filament.vendor.name={brand}";

            if (!string.IsNullOrEmpty(material))
            {
                query += $"&filament.material={material}";
            }

            var url = $"{_baseUrl}/api/v1/spool?{query}";

            var allBrandSpools = await _httpClient.GetFromJsonAsync<List<Spool>>(url, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            if (allBrandSpools == null || allBrandSpools.Count == 0)
            {
                Console.WriteLine("⚠️ No spools found.");
                return null;
            }

            // Find the first spool matching the brand and color
            var matchingSpool = allBrandSpools.FirstOrDefault(spool => color.StartsWith($"#{spool.Filament.ColorHex}", StringComparison.OrdinalIgnoreCase) == true);

            return matchingSpool;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ Error fetching spools from Spoolman: {ex.Message}");
            return null;
        }
    }
}
