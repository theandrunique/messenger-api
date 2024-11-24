using ErrorOr;
using FluentValidation.Results;

namespace MessengerAPI.Application.Common.Extensions;

public static class FluentValidationExtensions
{
    public static List<Error> ToErrorOrList(this List<ValidationFailure> error)
    {
        return error.ConvertAll(validationFailure => Error.Validation(
                                validationFailure.PropertyName,
                                validationFailure.ErrorMessage));
    }
}