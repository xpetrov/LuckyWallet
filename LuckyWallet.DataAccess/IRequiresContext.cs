namespace LuckyWallet.DataAccess;

public interface IRequiresContext
{
    DatabaseContext DbContext { get; }
}