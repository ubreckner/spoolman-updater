
namespace Gateways;

public class SpoolmanClient(IHealthEndpoint healthEndpoint, ISpoolEndpoint spoolEndpoint, IFieldEndpoint fieldEndpoint)
{
    public async Task<bool> UseSpoolWeightAsync(int spoolId, float usedWeight) =>
        await spoolEndpoint.UseSpoolWeight(spoolId, usedWeight);

    public async Task<Spool?> GetSpoolByBrandAndColorAsync(string brand, string material, string color, string tagUid) =>
        await spoolEndpoint.GetOrCreateSpool(brand, material, color, tagUid);

    public async Task<bool> CheckHealthAsync() =>
        await healthEndpoint.CheckHealthAsync();

    public async Task CheckFieldExistence() =>
        await fieldEndpoint.CheckFieldExistence();
}
