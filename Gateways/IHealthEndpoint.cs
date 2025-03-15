namespace Gateways;

public interface IHealthEndpoint
{
    Task<bool> CheckHealthAsync();
}