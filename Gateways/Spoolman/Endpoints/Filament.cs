namespace Gateways;

internal class FilamentSpoolManEndoint(SpoolmanConfiguration configuration) : SpoolmanEndoint<Filament>(configuration), IFilamentEndpoint
{
    protected override string Endpoint => "filament";

    // Get or create a vendor
    public async Task<Filament> GetOrCreateFilament(Vendor vendor, string color, string material)
    {
        Filament? filament = await GetFilament(vendor.Name, color, material);

        filament ??= await CreateFilament(color, material, vendor);

        return filament ?? throw new InvalidOperationException("Failed to create or retrieve filament.");
    }


    private async Task<Filament?> CreateFilament(string color, string material, Vendor vendor)
    {
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

        return await PostAsync(newFilament);
    }

    private async Task<Filament?> GetFilament(string vendorName, string color, string material)
    {
        var filaments = await GetAllAsync($"vendor.name={vendorName}&color_hex={color}&material={material}");

        Filament? filament = null;
        if (filaments != null && filaments.Any())
            filament = filaments.FirstOrDefault(filament => filament.ColorHex == color);

        return filament;
    }
}
