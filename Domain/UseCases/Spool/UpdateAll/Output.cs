using Gateways;

namespace Domain;

internal class UpdateAllSpoolsOutput : IOutput
{
    public UpdateAllSpoolsOutput(List<Spool> spools)
    {
        Spools = spools;
    }

    public List<Spool> Spools { get; }
}