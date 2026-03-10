namespace PharmaStock.BuildingBlocks.Common;

public record Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    private Result(bool isSuccess, string error)
    {
        if (isSuccess && !string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("Cannot create a successful result with an error message.", nameof(error));

        if (!isSuccess && string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("Cannot create a failed result with an empty error message.", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, string.Empty);

    public static Result Failure(string error) => new(false, error);

    public static implicit operator Result(string error) => Failure(error);
}

public record Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string error)
    {
        if (isSuccess && !string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("Cannot create a successful result with an error message.", nameof(error));

        if (!isSuccess && string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("Cannot create a failed result with an empty error message.", nameof(error));

        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, string.Empty);

    public static Result<T> Failure(string error) => new(false, default, error);

    public static implicit operator Result<T>(T value) => Success(value);

    public static implicit operator Result<T>(string error) => Failure(error);
}

