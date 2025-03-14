namespace Gateways;

public interface ISpoolmanEndpoint<TSpoolmanEntity>
    where TSpoolmanEntity : class
{
}

public interface IVendorEndpoint
{
    Task<Vendor> GetOrCreate(string name);
}

public interface IFilamentEndpoint
{
    Task<Filament> GetOrCreateFilament(Vendor vendor, string color, string material);
}

public interface ISpoolEndpoint
{
    Task<Spool> GetOrCreateSpool(string brand, string material, string color, string tagUid);

    Task<bool> UseSpoolWeight(int spoolId, float usedWeight);
}