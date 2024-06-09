using LuckyWallet.Controllers.Models;
using LuckyWallet.DataModel;

namespace LuckyWallet.Controllers.Mappings;

public static class TransactionMappings
{
    public static GetTransactionModel ToGetPlayerTransactionModel(Transaction transaction) =>
        new(transaction.Id, transaction.Amount, transaction.Type);
}
