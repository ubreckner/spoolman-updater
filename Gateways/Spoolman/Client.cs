namespace Gateways;

public class SpoolmanClient(ISpoolmanEndpoint<Spool> spoolEndpoint)
{
    private readonly ISpoolEndpoint spoolEndpoint = (ISpoolEndpoint)spoolEndpoint;

    public async Task<bool> UseSpoolWeightAsync(int spoolId, float usedWeight) =>
        await spoolEndpoint.UseSpoolWeight(spoolId, usedWeight);

    public async Task<Spool?> GetSpoolByBrandAndColorAsync(string brand, string material, string color, string tagUid) =>
        await spoolEndpoint.GetOrCreateSpool(brand, material, color, tagUid);
}
