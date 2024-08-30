using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sample.Application.Persons;
using Sample.Application.Persons.GetPersons;
using Sample.Application.Persons.ImportPersons;
using Sample.Domain.Abstractions;

namespace Sample.Console;
internal sealed class ConsoleHostedService : IHostedService
{
    private readonly ILogger<ConsoleHostedService> _logger;
    private readonly ISender _sender;

    public ConsoleHostedService(
        ILogger<ConsoleHostedService> logger,
        ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

        try
        {
            Result commandResult = await _sender.Send(
                new ImportPersonsCommand(),
                cancellationToken);
            if (commandResult.IsFailure)
            {
                _logger.LogError(commandResult.Error.Name);
                return;
            }

            Result<IReadOnlyCollection<PersonResponse>> queryResult = await _sender.Send(
                new GetPersonsQuery(),
                cancellationToken);
            if (queryResult.IsFailure)
            {
                _logger.LogError(queryResult.Error.Name);
                return;
            }
            foreach (PersonResponse person in queryResult.Value)
            {
                System.Console.WriteLine(person);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong, message {message}", e.Message);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
