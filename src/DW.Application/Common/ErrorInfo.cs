namespace DW.Application.Common;

public class ErrorInfo
{
    public string Code { get; }
    public string Message { get; }
    public string Field { get; }
    
    public ErrorInfo(string code, string message, string field = null)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Field = field;
    }
    
    public static ErrorInfo ValidationError(string code, string message, string field) =>
        new(code, message, field);
    
    public static ErrorInfo General(string code, string message) =>
        new(code, message);
    
    public override string ToString() =>
        string.IsNullOrEmpty(Field) ? $"{Code}: {Message}" : $"{Code}: {Message} (Field: {Field})";
}