namespace Messenger.Errors;

public readonly struct ErrorOr<TValue>
{
    private readonly TValue _result;
    private readonly ApiError _error;

    public bool IsError { get; }
    public TValue Value => !IsError ? _result : throw new InvalidOperationException("Value is not available when IsError is true.");
    public ApiError Error => IsError ? _error : throw new InvalidOperationException("Error is not available when IsError is false.");

    public ErrorOr(ApiError error)
    {
        IsError = true;
        _error = error;
        _result = default!;
    }

    public ErrorOr(TValue value)
    {
        IsError = false;
        _result = value;
        _error = default!;
    }

    public TNextValue Match<TNextValue>(Func<TValue, TNextValue> onValue, Func<ApiError, TNextValue> onError)
    {
        if (IsError)
        {
            return onError(Error);
        }

        return onValue(Value);
    }

    public static implicit operator ErrorOr<TValue>(TValue value) => new ErrorOr<TValue>(value);

    public static implicit operator ErrorOr<TValue>(ApiError error) => new ErrorOr<TValue>(error);
}

