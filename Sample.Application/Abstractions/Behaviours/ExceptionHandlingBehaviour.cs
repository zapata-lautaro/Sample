using MediatR;
using Microsoft.Extensions.Logging;
using Sample.Application.Exceptions;

namespace Sample.Application.Abstractions.Behaviours;

internal sealed class ExceptionHandlingBehavior<TRequest, TResponse>(
    ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
where TRequest : class
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception for command {command}", typeof(TRequest).Name);

            throw new SampleException(typeof(TRequest).Name, innerException: exception);
        }
    }
}
