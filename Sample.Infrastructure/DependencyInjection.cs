using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sample.Application.Abstractions.Data;
using Sample.Application.Abstractions.PersonService;
using Sample.Domain.Abstractions;
using Sample.Domain.Persons;
using Sample.Infrastructure.Data;
using Sample.Infrastructure.PersonService;
using Sample.Infrastructure.Repositories;

namespace Sample.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IPersonsService, HttpPersonService>((serviceProvider, httpClient) =>
        {
            HttpPersonServiceOptions personsServiceSettings = serviceProvider.GetRequiredService<IOptions<HttpPersonServiceOptions>>().Value;

            httpClient.BaseAddress = new Uri(personsServiceSettings.Url);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(4),
            };
        })
        .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        string connectionString =
            configuration.GetConnectionString("Database") ??
            throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IPersonRepository, PersonRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        return services;
    }
}
