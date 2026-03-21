using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using PharmaStock.BuildingBlocks.Common;

namespace PharmaStock.BuildingBlocks.Validation;

public sealed class FluentValidationMediatorPipelineBehavior<TMessage, TResponse>(
    IServiceProvider serviceProvider)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage, IValidatableRequest
{
    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        var validators = serviceProvider.GetServices<IValidator<TMessage>>().ToList();
        if (validators.Count == 0)
            return await next(message, cancellationToken);

        var errors = new List<string>();
        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(message, cancellationToken);
            if (!result.IsValid)
                errors.AddRange(result.Errors.Select(e => e.ErrorMessage));
        }

        if (errors.Count == 0)
            return await next(message, cancellationToken);

        var errorMessage = string.Join("; ", errors);
        return TryCreateFailureResponse(errorMessage, out var failureResponse)
            ? failureResponse
            : throw new InvalidOperationException(
                $"Validation failed for {typeof(TMessage).Name}, but TResponse does not support {nameof(Result)} pattern. Error: {errorMessage}");
    }

    private static bool TryCreateFailureResponse(string errorMessage, out TResponse failureResponse)
    {
        var responseType = typeof(TResponse);

        if (responseType == typeof(Result))
        {
            failureResponse = (TResponse)(object)Result.Failure(errorMessage);
            return true;
        }

        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = responseType.GetGenericArguments()[0];
            var constructedResultType = typeof(Result<>).MakeGenericType(valueType);
            var failureMethod = constructedResultType.GetMethod(
                "Failure",
                [typeof(string)]);

            if (failureMethod is null)
            {
                failureResponse = default!;
                return false;
            }

            failureResponse = (TResponse)failureMethod.Invoke(null, [errorMessage])!;
            return true;
        }

        failureResponse = default!;
        return false;
    }
}

