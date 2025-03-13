namespace Gateways;


using System.Collections.Generic;
using System.Drawing;

public class Filament
{
    public int Id { get; set; }
    public string Registered { get; set; }
    public string Name { get; set; }
    public int VendorId { get; set; }
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

    public static string GetNearestColorName(string hex)
    {
        Color target = ColorTranslator.FromHtml(hex);

        return typeof(Color).GetProperties()
            .Where(p => p.PropertyType == typeof(Color))
            .Select(p => (Color)p.GetValue(null))
            .Where(c => c.A == 255)  // Exclude transparent colors
            .OrderBy(c => GetColorDistance(target, c))
            .First().Name;
    }

    private static double GetColorDistance(Color c1, Color c2)
    {
        return Math.Sqrt(
            Math.Pow(c1.R - c2.R, 2) +
            Math.Pow(c1.G - c2.G, 2) +
            Math.Pow(c1.B - c2.B, 2)
        );
    }
}
