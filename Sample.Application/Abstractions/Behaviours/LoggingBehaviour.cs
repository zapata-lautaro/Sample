using MediatR;
using Microsoft.Extensions.Logging;
using Sample.Application.Abstractions.Messaging;

namespace Sample.Application.Abstractions.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string name = request.GetType().Name;

        try
        {
            _logger.LogInformation("Executing command {Command}", name);

            TResponse? result = await next();

            _logger.LogInformation("Command {Command} processed successfully", name);

            return result;
        }
        catch (Exception)
        {
            _logger.LogError("Command {Command} processing failed", name);

            throw;
        }
    }
}
