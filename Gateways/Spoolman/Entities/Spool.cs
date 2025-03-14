namespace Gateways;

using System.Collections.Generic;

public class Spool
{
    public int Id { get; set; }
    public string Registered { get; set; }
    public string FirstUsed { get; set; }
    public string LastUsed { get; set; }
    public int FilamentId { get; set; }
    public Filament Filament { get; set; }
    public decimal? Price { get; set; }
    public double RemainingWeight { get; set; }
    public double InitialWeight { get; set; }
    public double SpoolWeight { get; set; }
    public double? UsedWeight { get; set; }
    public double? RemainingLength { get; set; }
    public double UsedLength { get; set; }
    public string Location { get; set; }
    public string LotNumber { get; set; }
    public string Comment { get; set; }
    public bool Archived { get; set; }
    public Dictionary<string, string> Extra { get; set; }
}
