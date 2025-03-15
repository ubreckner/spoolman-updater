using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gateways;

internal abstract class SpoolmanEndpoint<TSpoolmanEntity> : ISpoolmanEndpoint<TSpoolmanEntity>
    where TSpoolmanEntity : class
{
    protected readonly HttpClient HttpClient;
    protected readonly JsonSerializerOptions JsonOptions;

    protected abstract string Endpoint { get; }

    public SpoolmanEndpoint(SpoolmanConfiguration configuration)
    {
        HttpClient = new HttpClient();
        HttpClient.BaseAddress = new Uri($"{configuration.Url}/api/v1/");

        // Configure snake_case naming policy
        JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<List<TSpoolmanEntity>?> GetAllAsync(string query = "", bool useQueryParams = true) =>
        await HttpClient.GetFromJsonAsync<List<TSpoolmanEntity>>($"{Endpoint}{(useQueryParams ? "?" : string.Empty)}{query}", JsonOptions);

    public async Task<TSpoolmanEntity?> PostAsync(TSpoolmanEntity newEntity)
    {
        var createVendorResponse = await HttpClient.PostAsJsonAsync(Endpoint, newEntity, JsonOptions);

        return createVendorResponse.IsSuccessStatusCode ? await createVendorResponse.Content.ReadFromJsonAsync<TSpoolmanEntity>() : null;
    }
}