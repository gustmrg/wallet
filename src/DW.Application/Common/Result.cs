namespace DW.Application.Common;

/// <summary>
/// Represents the outcome of an operation with support for success/failure states,
/// optional data, and multiple error messages.
/// This is the non-generic base class for operations that don't return data.
/// </summary>
public class Result
{
    protected Result(bool isSuccess, IEnumerable<Error> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors?.ToList() ?? new List<Error>();
    }

    /// <summary>
    /// Indicates whether the operation was successful
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indicates whether the operation failed
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Collection of error messages if the operation failed
    /// </summary>
    public List<Error> Errors { get; }
    
    /// <summary>
    /// Checks if the result contains a specific error code
    /// </summary>
    public bool HasError(string errorCode)
    {
        return Errors.Any(e => e.Code == errorCode);
    }

    /// <summary>
    /// Creates a successful result with no data
    /// </summary>
    public static Result Success()
    {
        return new Result(true, new List<Error>());
    }

    /// <summary>
    /// Creates a failed result with a single error message
    /// </summary>
    public static Result Failure(Error error)
    {
        return new Result(false, new List<Error> { error });
    }

    /// <summary>
    /// Creates a failed result with multiple error messages
    /// </summary>
    public static Result Failure(IEnumerable<Error> errors)
    {
        return new Result(false, errors);
    }

    /// <summary>
    /// Creates a successful result with data (generic version)
    /// This is a convenience method that creates a Result<T>
    /// </summary>
    public static Result<T> Success<T>(T data)
    {
        return Result<T>.Success(data);
    }

    /// <summary>
    /// Creates a failed result with data type (generic version)
    /// This is a convenience method that creates a Result<T>
    /// </summary>
    public static Result<T> Failure<T>(Error error)
    {
        return Result<T>.Failure(error);
    }

    /// <summary>
    /// Creates a failed result with multiple errors and data type (generic version)
    /// </summary>
    public static Result<T> Failure<T>(IEnumerable<Error> errors)
    {
        return Result<T>.Failure(errors);
    }
}

/// <summary>
/// Generic version of Result that can carry data of type T
/// Used when an operation needs to return a value upon success
/// </summary>
/// <typeparam name="T">The type of data returned on success</typeparam>
public class Result<T> : Result
{
    private Result(bool isSuccess, T data, IEnumerable<Error> errors)
        : base(isSuccess, errors)
    {
        Data = data;
    }

    /// <summary>
    /// The data returned by the operation if successful
    /// Will be default(T) if the operation failed
    /// </summary>
    public T Data { get; }

    /// <summary>
    /// Creates a successful result with the specified data
    /// </summary>
    public static Result<T> Success(T data)
    {
        return new Result<T>(true, data, new List<Error>());
    }

    /// <summary>
    /// Creates a failed result with a single error message
    /// Data will be set to default(T)
    /// </summary>
    public new static Result<T> Failure(Error error)
    {
        return new Result<T>(false, default(T), new List<Error> { error });
    }

    /// <summary>
    /// Creates a failed result with multiple error messages
    /// Data will be set to default(T)
    /// </summary>
    public new static Result<T> Failure(IEnumerable<Error> errors)
    {
        return new Result<T>(false, default(T), errors);
    }

    /// <summary>
    /// Implicit conversion from T to Result<T>
    /// Allows to return a value directly, and it will be wrapped in a successful Result
    /// Example: Result<User> GetUser() => user; // Automatically wrapped
    /// </summary>
    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }
}