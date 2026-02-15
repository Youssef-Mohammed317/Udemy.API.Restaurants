using FluentValidation;
using MediatR;

namespace Restaurants.Application.Common;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(_validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failuers = results.SelectMany(r => r.Errors)
            .Where(e => e is not null).ToList();

        if (failuers.Any())
        {
            throw new ValidationException(failuers);
        }

        return await next();

    }
}
