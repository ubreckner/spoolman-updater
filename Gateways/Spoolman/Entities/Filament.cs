namespace Gateways;


using System.Collections.Generic;

public class Filament
{
    public int Id { get; set; }
    public string Registered { get; set; }
    public string Name { get; set; }
    public Vendor Vendor { get; set; }
    public string Material { get; set; }
    public decimal Price { get; set; }
    public double Density { get; set; }
    public double Diameter { get; set; }
    public double Weight { get; set; }
    public double SpoolWeight { get; set; }
    public string ArticleNumber { get; set; }
    public string Comment { get; set; }
    public int ExtruderTemp { get; set; }
    public int BedTemp { get; set; }
    public string ColorHex { get; set; }
    public string MultiColorHexes { get; set; }
    public string MultiColorDirection { get; set; }
    public string ExternalId { get; set; }
    public Dictionary<string, string> Extra { get; set; }
}
