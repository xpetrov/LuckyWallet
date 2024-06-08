using System.Net;

namespace LuckyWallet.Controllers.Infrastructure;

public static class OperationResult
{
    public static Success<T> Success<T>(T value) => new(value);

    public static OperationError<T> OperationError<T>(string message, HttpStatusCode statusCode) =>
        new(message, statusCode);
}

public abstract record OperationResult<T>();

public sealed record Success<T>(T Value) : OperationResult<T>;

public sealed record OperationError<T> : OperationResult<T>
{
    internal OperationError(string message, HttpStatusCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }

    public string Message { get; }

    public HttpStatusCode StatusCode { get; }
}
