namespace TT.Core;

public struct Result<TValue, TError>
{
    public TValue Value;
    public TError Error;
    public bool IsSuccess => _success;

    private bool _success;

    public static Result<TValue, TError> Success(TValue value) => new Result<TValue, TError>(value, default(TError), true);
    public static Result<TValue, TError> Failed(TError error) => new Result<TValue, TError>(default(TValue), error, false);

    public TCompare Match<TCompare>(Func<TValue, TCompare> success, Func<TError, TCompare> failure) 
    {
        return _success 
            ? success(Value) 
            : failure(Error);
    }

    public static implicit operator Result<TValue, TError>(TValue value) => Success(value);
    public static implicit operator Result<TValue, TError>(TError error) => Failed(error);

    private Result(TValue value, TError error, bool isSuccess) 
    {
        Value = value;
        Error = error;
        _success = isSuccess;
    }
}
