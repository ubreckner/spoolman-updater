using System.Net.Http.Json;

namespace Gateways;

public class HomeAssistantClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _token;

    public HomeAssistantClient(HomeAssistantConfiguration configuration)
    {
        _baseUrl = configuration.Url;
        _token = configuration.Token;

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
    }

    public async Task<TrayInfo?> GetTrayInfoAsync(int trayIndex)
    {
        string sensorEntity = $"sensor.x1c_00m09c422100420_ams_1_tray_{trayIndex}";

        var response = await _httpClient.GetFromJsonAsync<HomeAssistantState>($"{_baseUrl}/api/states/{sensorEntity}");

        return response?.Attributes;
    }

    public async Task<List<TrayInfo?>> GetAllTrayInfoAsync()
    {
        var trayTasks = new List<Task<TrayInfo?>>
        {
            GetTrayInfoAsync(1),
            GetTrayInfoAsync(2),
            GetTrayInfoAsync(3),
            GetTrayInfoAsync(4)
        };

        var results = await Task.WhenAll(trayTasks);

        return new List<TrayInfo?>(results);
    }
}
