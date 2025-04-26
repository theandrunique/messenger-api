using FluentValidation;
using MediatR;
using Messenger.Errors;

namespace Messenger.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    /// <summary>
    /// Check if handler has a validator and validate request
    /// </summary>
    /// <param name="request"><see cref="TRequest"/></param>
    /// <param name="next"><see cref="RequestHandlerDelegate<TResponse>"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="TResponse"/></returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator == null)
            return await next();

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        Dictionary<string, List<string>> fieldErrors = new();

        foreach (var fieldError in validationResult.Errors)
        {
            if (!fieldErrors.ContainsKey(fieldError.PropertyName))
            {
                fieldErrors[fieldError.PropertyName] = new();
            }
            fieldErrors[fieldError.PropertyName].Add(fieldError.ErrorMessage);
        }

        var error = ApiErrors.Common.InvalidRequestBody(fieldErrors);

        return (dynamic)error;
    }
}
