using Guardian.Agent;
using Microsoft.Extensions.Hosting;
using Guardian.Common.Interfaces;
using Guardian.Scanner.RegistryScanner;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<ISoftwareScanner, RegistrySoftwareScanner>();
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "Guardian Agent";
});

var host = builder.Build();
host.Run();
