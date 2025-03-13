using Gateways;

namespace Updater;

public class SpoolmanUpdater(HomeAssistantClient homeAssistantClient, SpoolmanClient spoolmanClient)
{
    public async Task UpdateSpools()
    {
        var trayInfos = await homeAssistantClient.GetAllTrayInfoAsync();

        foreach(var trayInfo in trayInfos)
        {
            if (trayInfo == null)
                continue;

            string trayName = trayInfo.Name ?? "Unknown"; // Handle null values
            string[] parts = trayName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string brand = parts.Length > 0 ? parts[0] : "Unknown";
            string material = parts.Length > 1 ? parts[1] : "Unknown";

            var spool = await spoolmanClient.GetSpoolByBrandAndColorAsync(brand, material, trayInfo.Color, trayInfo.TagUid);
        }
    }
}
