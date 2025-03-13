using Gateways;

namespace Domain;

internal sealed class UpdateAllSpoolsUseCase(HomeAssistantClient homeAssistantClient, SpoolmanClient spoolmanClient) : IUseCase<UpdateAllSpoolsInput>
{
    public async Task<IOutput> ExecuteAsync(UpdateAllSpoolsInput input)
    {
        var trayInfos = await homeAssistantClient.GetAllTrayInfoAsync();

        foreach (var trayInfo in trayInfos)
        {
            if (trayInfo == null)
                continue;

            string trayName = trayInfo.Name ?? "Unknown"; // Handle null values
            string[] parts = trayName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string brand = parts.Length > 0 ? parts[0] : "Unknown";

            var spool = await spoolmanClient.GetSpoolByBrandAndColorAsync(brand, trayInfo.Type, trayInfo.Color, trayInfo.TagUid);
        }

        return new UpdateAllSpoolsOutput();
    }
}