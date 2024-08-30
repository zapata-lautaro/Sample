using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sample.Application;
using Sample.Infrastructure;
using Sample.Infrastructure.PersonService;

namespace Sample.Console;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IHost app = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ConsoleHostedService>();
                services.Configure<HttpPersonServiceOptions>(
                    hostContext.Configuration.GetSection(HttpPersonServiceOptions.HttpPersonService));
                services.AddApplication();
                services.AddInfrastructure(hostContext.Configuration);
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                });
            })
            .Build();

        ConsoleHostedService consoleApp = app.Services.GetRequiredService<ConsoleHostedService>();
        await consoleApp.StartAsync();
    }
}
