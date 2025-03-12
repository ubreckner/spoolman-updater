using Gateways;

namespace Updater;

public class UpdaterConfiguration
{
    public SpoolmanConfiguration Spoolman { get; set; } = new();

    public HomeAssistantConfiguration HomeAssistant { get; set; } = new();
}
