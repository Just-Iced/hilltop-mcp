using Azure.Monitor.OpenTelemetry.Exporter;
using McpFunc.McpTools;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var studentDataUrl = builder.Configuration["StudentDataUrl"]
    ?? "https://gist.githubusercontent.com/medhatelmasry/2ae7a36d5392f862b0d1039b2a52b9b4/raw/";

builder.Services.AddHttpClient<StudentDirectoryClient>(client =>
{
    client.BaseAddress = new Uri(studentDataUrl);
    client.Timeout = TimeSpan.FromSeconds(15);
});

if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING")))
{
    builder.Services.AddOpenTelemetry()
        .UseFunctionsWorkerDefaults()
        .UseAzureMonitorExporter();
}

builder.ConfigureMcpTool(McpToolDefinitions.StudentCountTool.Name);
builder.ConfigureMcpTool(McpToolDefinitions.YoungestStudentTool.Name);
builder.ConfigureMcpTool(McpToolDefinitions.StudentEnrollmentTool.Name);

builder.Build().Run();
