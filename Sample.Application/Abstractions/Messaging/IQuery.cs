using MediatR;
using Sample.Domain.Abstractions;

namespace Sample.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
