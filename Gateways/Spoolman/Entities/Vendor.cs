namespace Gateways;


using System.Collections.Generic;

public class Vendor
{
    public int Id { get; set; }
    public string Registered { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }
    public double EmptySpoolWeight { get; set; }
    public string ExternalId { get; set; }
    public Dictionary<string, string> Extra { get; set; }
}