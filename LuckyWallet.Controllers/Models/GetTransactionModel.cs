using LuckyWallet.DataModel.Enums;

namespace LuckyWallet.Controllers.Models;

public record GetTransactionModel(Guid Id, decimal Amount, TransactionType Type);
