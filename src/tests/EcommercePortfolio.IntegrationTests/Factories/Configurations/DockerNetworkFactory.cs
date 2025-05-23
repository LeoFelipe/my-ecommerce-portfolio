using Docker.DotNet;
using Docker.DotNet.Models;

namespace EcommercePortfolio.IntegrationTests.Factories.Configurations;

public static class DockerNetworkFactory
{
    public static async Task EnsureNetworkExistsAsync(string networkName = "ecommerce_network")
    {
        var dockerClient = new DockerClientConfiguration().CreateClient();
        var networks = await dockerClient.Networks.ListNetworksAsync();

        if (!networks.Any(n => n.Name == networkName))
        {
            await dockerClient.Networks.CreateNetworkAsync(new NetworksCreateParameters
            {
                Name = networkName
            });
        }
    }
}
