// See https://aka.ms/new-console-template for more information
using EfPostgresql;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
    })
    .ConfigureServices(services =>
    {
        services.AddApplication<Application>();
    })
    .UseAutofac();

await builder.RunConsoleAsync();