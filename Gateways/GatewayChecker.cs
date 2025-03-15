namespace Gateways;

public class GatewayChecker(SpoolmanClient SpoolmanClient)
{
    public async Task<bool> CheckGatewayConnectionAsync()
    {
        if (!await SpoolmanClient.CheckHealthAsync())
        {
            Console.WriteLine("Spoolman is not available.");
            return false;
        }

        await SpoolmanClient.CheckFieldExistence();

        return true;
    }
}
