using System.Net;

namespace LuckyWallet.Controllers.Infrastructure;

[Serializable]
public class OperationErrorException(HttpStatusCode statusCode, string message) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}
