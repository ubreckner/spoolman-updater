using Gateways;

namespace Domain;

internal sealed class UpdateSpoolUseCase(SpoolmanClient spoolmanClient) : IUseCase<UpdateSpoolInput>
{
    public async Task<IOutput> ExecuteAsync(UpdateSpoolInput input)
    {
        string[] parts = input.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        string brand = parts.Length > 0 ? parts[0] : "Unknown";

        var spool = await spoolmanClient.GetSpoolByBrandAndColorAsync(brand, input.Material, input.Color, input.TagUid);
        if (spool == null)
            return new UpdateSpoolOutput(false);

        var success = await spoolmanClient.UseSpoolWeightAsync(spool.Id, input.UsedWeight);

        return new UpdateSpoolOutput(success);
    }
}