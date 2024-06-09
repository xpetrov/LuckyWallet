using LuckyWallet.DataModel.Enums;

namespace LuckyWallet.DataModel;

public class Transaction
{
    public Guid Id { get; set; } // Primary Key

    public Guid UniqueTransactionId { get; set; } // Sent by the client

    public Guid WalletId { get; set; }

    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    public TransactionResult Result { get; set; }

    public Wallet Wallet { get; set; } = null!;
}