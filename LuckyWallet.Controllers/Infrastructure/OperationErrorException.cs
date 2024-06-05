namespace LuckyWallet.Controllers.Infrastructure;

[Serializable]
public class OperationErrorException(string message) : Exception(message)
{
}
