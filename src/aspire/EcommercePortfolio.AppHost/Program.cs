using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.EcommercePortfolio_Carts_API>("ecommerceportfolio-carts-api");

await builder.Build().RunAsync();
