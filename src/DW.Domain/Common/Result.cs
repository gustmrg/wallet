namespace DW.Domain.Common;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T Data { get; private set; }
    public List<string> Errors { get; private set; } = new();
    public ErrorType? Type { get; private set; }
    
    public static Result<T> Success(T data) => new()
    { 
        IsSuccess = true, 
        Data = data, 
    };
    
    public static Result<T> Failure(string error, ErrorType type) => new()
    { 
        IsSuccess = false, 
        Type = type,
        Errors = new List<string> { error }
    };
}

public enum ErrorType
{
    NotFound,
    ValidationError,
    BusinessLogicError,
    Unauthorized,
    Forbidden
}