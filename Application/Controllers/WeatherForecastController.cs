using Gateways;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("[controller]")]
public class SpoolsController(HomeAssistantClient homeAssistantClient, SpoolmanClient spoolmanClient) : ControllerBase
{
    [HttpGet()]
    public async Task UpdateSpools()
    {
        var trayInfos = await homeAssistantClient.GetAllTrayInfoAsync();

        foreach (var trayInfo in trayInfos)
        {
            if (trayInfo == null)
                continue;

            string trayName = trayInfo.Name ?? "Unknown"; // Handle null values
            string[] parts = trayName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string brand = parts.Length > 0 ? parts[0] : "Unknown";
            string material = parts.Length > 1 ? parts[1] : "Unknown";

            var spool = await spoolmanClient.GetSpoolByBrandAndColorAsync(brand, material, trayInfo.Color);
        }
    }

    [HttpGet("spool")]
    public async Task<bool?> UpdateSpool(string name, string color, float usedWeight)
    {
        string[] parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        string brand = parts.Length > 0 ? parts[0] : "Unknown";
        string material = parts.Length > 1 ? parts[1] : "Unknown";

        var spool = await spoolmanClient.GetSpoolByBrandAndColorAsync(brand, material, color);
        if (spool == null)
            return null;

        return await spoolmanClient.UseSpoolWeightAsync(spool.Id, usedWeight);
    }
}
