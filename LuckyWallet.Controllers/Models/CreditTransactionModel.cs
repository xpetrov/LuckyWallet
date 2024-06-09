using LuckyWallet.DataModel.Enums;

namespace LuckyWallet.Controllers.Models;

public record CreditTransactionModel(
    Guid UniqueTransactionId,
    Guid PlayerId,
    TransactionType Type,
    decimal Amount);