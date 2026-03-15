using Mediator;

namespace PharmaStock.Tests.Common.Base;

public abstract class CommandHandlerTestBase<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected static CancellationToken Ct => CancellationToken.None;

    protected abstract IRequestHandler<TRequest, TResponse> CreateHandler();
}
