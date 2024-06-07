using LuckyWallet.DataModel.Enums;

namespace LuckyWallet.DataModel;

public class Transaction
{
    public Guid Id { get; set; }

    public Guid UniqueTransactionId { get; set; }

    public Guid WalletId { get; set; }

    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    public Wallet Wallet { get; set; } = null!;
}