using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.EnableMcpToolMetadata();

builder
    .ConfigureMcpTool("search_customers")
    .WithProperty("query", "string", "Mandatory! Search query string to find customers by name, organization number, or other fields.", required: true);

builder.Build().Run();
