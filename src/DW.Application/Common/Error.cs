namespace DW.Application.Common;

public class Error
{
    public string Code { get; }

    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error NotFound(string message = "Resource not found")
        => new("NOT_FOUND", message);

    public static Error BadRequest(string message = "Bad request")
        => new("BAD_REQUEST", message);

    public static Error Unauthorized(string message = "Unauthorized")
        => new("UNAUTHORIZED", message);

    public static Error Forbidden(string message = "Forbidden")
        => new("FORBIDDEN", message);

    public static Error InternalServerError(string message = "Internal server error")
        => new("INTERNAL_SERVER_ERROR", message);

    public static Error ValidationFailed(string message = "Validation failed")
        => new("VALIDATION_FAILED", message);

    public static Error Conflict(string message = "Conflict")
        => new("CONFLICT", message);

    public static Error Custom(string code, string message)
        => new(code, message);
}