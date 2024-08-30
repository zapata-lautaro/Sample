using Microsoft.Extensions.DependencyInjection;
using Sample.Application.Abstractions.Behaviours;

namespace Sample.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            configuration.AddOpenBehavior(typeof(LoggingBehaviour<,>));

            configuration.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
        });

        return services;
    }
}
