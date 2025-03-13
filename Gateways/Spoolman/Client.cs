using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gateways;

public class SpoolmanClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly string _baseUrl;

    public SpoolmanClient(SpoolmanConfiguration configuration)
    {
        _baseUrl = configuration.Url;

        _httpClient = new HttpClient();

        // Configure snake_case naming policy
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<bool> UseSpoolWeightAsync(int spoolId, float usedWeight)
    {
        var payload = new { use_weight = usedWeight };
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/v1/spool/{spoolId}/use", payload);

        return response.IsSuccessStatusCode;
    }

    public async Task<Spool?> GetSpoolByBrandAndColorAsync(string brand, string material, string color, string tagUid)
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

        var url = $"{_baseUrl}/api/v1/spool?{query}";

        var allBrandSpools = await _httpClient.GetFromJsonAsync<List<Spool>>(url, _jsonOptions);

        if (allBrandSpools == null || allBrandSpools.Count == 0)
        {
            Console.WriteLine("⚠️ No spools found.");
            return null;
        }

        // Find the first spool matching the brand and color
        var matchingSpool = allBrandSpools.FirstOrDefault(spool => color.StartsWith($"#{spool.Filament.ColorHex}", StringComparison.OrdinalIgnoreCase) == true);

        if (matchingSpool == null)
        {
            matchingSpool = await CreateSpoolAsync(brand, color.Substring(1, 6), material);
        }

        return matchingSpool;
    }

    public async Task<Spool?> CreateSpoolAsync(string vendor, string color, string material)
    {
        var filament = await GetOrCreateFilament(vendor, color, material);

        var newSpool = new Spool
        {
            FilamentId = filament.Id,
            InitialWeight = 1000,  // Default values, adjust as needed
            RemainingWeight = 1000,
            SpoolWeight = 250
        };

        var createResponse = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/v1/spool", newSpool, _jsonOptions);

        if (createResponse.IsSuccessStatusCode)
        {
            return await createResponse.Content.ReadFromJsonAsync<Spool>();  // Return the created spool
        }

        return null;
    }

    public async Task<Filament> GetOrCreateFilament(string brand, string color, string material)
    {
        var filamentResponse = await _httpClient.GetFromJsonAsync<List<Filament>>(
            $"{_baseUrl}/api/v1/filament?vendor.name={brand}&color_hex={color}&material={material}", _jsonOptions
        );

        Filament? filament = null;
        if (filamentResponse != null && filamentResponse.Any())
            filament = filamentResponse.FirstOrDefault(filament => filament.ColorHex == color);
       
        if (filament == null)
        {
            var vendor = await GetOrCreateVendor(brand);

            // Step 2: Filament does not exist, create it
            var newFilament = new Filament
            {
                Name = Filament.GetNearestColorName($"#{color}"),  // Default name, adjust as needed
                VendorId = vendor.Id,
                ColorHex = color,
                Material = material,  // Default material, adjust as needed
                Diameter = 1.75,
                Density = 1.24,
                Weight = 1000
            };

            var createFilamentResponse = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/v1/filament", newFilament, _jsonOptions);

            if (!createFilamentResponse.IsSuccessStatusCode)
            {
                return null; // Filament creation failed
            }

            filament = await createFilamentResponse.Content.ReadFromJsonAsync<Filament>();
        }

        return filament;
    }

    // Get or create a vendor
    public async Task<Vendor> GetOrCreateVendor(string name)
    {
        var vendorResponse = await _httpClient.GetFromJsonAsync<List<Vendor>>($"{_baseUrl}/api/v1/vendor?name={name}", _jsonOptions);
        Vendor vendor;
        if (vendorResponse != null && vendorResponse.Any())
            vendor = vendorResponse.First();
        else
        {
            var newVendor = new Vendor
            {
                Name = name
            };

            var createVendorResponse = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/v1/vendor", newVendor, _jsonOptions);

            if (!createVendorResponse.IsSuccessStatusCode)
            {
                return null; // Vendor creation failed
            }

            vendor = await createVendorResponse.Content.ReadFromJsonAsync<Vendor>();
        }

        return vendor;
    }
}
