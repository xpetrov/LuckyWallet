using System.Net;

namespace LuckyWallet.Controllers.Infrastructure;

public static class NullableExtensions
{
    public static T ThrowIfNull<T>(this T? input, string errorDescription, HttpStatusCode statusCode = HttpStatusCode.NotFound) where T : struct =>
        input ?? throw new OperationErrorException(statusCode, errorDescription);

    public static async Task<T> ThrowIfNullAsync<T>(this Task<T?> input, string errorDescription, HttpStatusCode statusCode = HttpStatusCode.NotFound) where T : struct
    {
        var result = await input;

        return result.ThrowIfNull(errorDescription, statusCode);
    }

    public static T ThrowIfNull<T>(this T? input, string errorDescription, HttpStatusCode statusCode = HttpStatusCode.NotFound) where T : class =>
        input ?? throw new OperationErrorException(statusCode, errorDescription);

    public static async Task<T> ThrowIfNullAsync<T>(this Task<T?> input, string errorDescription, HttpStatusCode statusCode = HttpStatusCode.NotFound) where T : class
    {
        var result = await input;

        return result.ThrowIfNull(errorDescription, statusCode);
    }
}
