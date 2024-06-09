using LuckyWallet.Controllers.Infrastructure;
using LuckyWallet.Controllers.Models;
using LuckyWallet.DataAccess;
using LuckyWallet.DataModel;
using LuckyWallet.DataModel.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Security.Claims;

namespace LuckyWallet.Controllers.Operations;

public class CreditTransactionOperation : OperationBase<CreditTransactionModel>
{
    private readonly ICreditTransactionDatabaseFacade _facade;
    private readonly IUnitOfWork _unitOfWork;

    public CreditTransactionOperation(ICreditTransactionDatabaseFacade facade, IUnitOfWork unitOfWork)
    {
        _facade = facade;
        _unitOfWork = unitOfWork;
    }

    public static IServiceCollection Register(IServiceCollection services) =>
        services
            .AddScoped<CreditTransactionOperation>()
            .AddScoped<ICreditTransactionDatabaseFacade, CreditTransactionDatabaseFacade>();

    protected override async Task<None> ExecuteCore(
        CreditTransactionModel input,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken)
    {
        //using var t = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var wallet = (await _facade.GetPlayerWallet(input.PlayerId, cancellationToken))
            .ThrowIfNull("No Wallet Found For a Player.");

        var pastTransaction = await _facade.GetTransaction(input.UniqueTransactionId, cancellationToken);

        if (pastTransaction is not null)
        {
            if (pastTransaction.Result == TransactionResult.Accepted)
            {
                return None.Value;
            }
            else if (pastTransaction.Result == TransactionResult.Rejected)
            {
                throw new OperationErrorException(
                    HttpStatusCode.Conflict,
                    "Rejected due to insufficient funds.");
            }
        }

        var newTransaction = new Transaction
        {
            UniqueTransactionId = input.UniqueTransactionId,
            WalletId = wallet.Id,
            Amount = input.Amount,
            Type = input.Type,
            Result = input.Type == TransactionType.Stake && wallet.Balance < input.Amount
                ? TransactionResult.Rejected
                : TransactionResult.Accepted
        };

        wallet.Balance = input.Type == TransactionType.Stake
            ? wallet.Balance - input.Amount
            : wallet.Balance + input.Amount;

        wallet.Transactions.Add(newTransaction);

        await _unitOfWork.SaveAsync(cancellationToken);
        //await t.CommitAsync(cancellationToken);

        if (newTransaction.Result == TransactionResult.Rejected)
        {
            throw new OperationErrorException(
                HttpStatusCode.Conflict,
                "Rejected due to insufficient funds.");
        }

        return None.Value;
    }

    public interface ICreditTransactionDatabaseFacade : IRequiresContext
    {
        Task<Transaction?> GetTransaction(Guid uniqueTransactionId, CancellationToken cancellationToken) =>
            DbContext.Set<Transaction>()
                .AsNoTracking()
                .Where(t => t.UniqueTransactionId == uniqueTransactionId)
                .FirstOrDefaultAsync(cancellationToken);

        Task<Wallet?> GetPlayerWallet(Guid playerId, CancellationToken cancellationToken) =>
            DbContext.Set<Wallet>()
                .Include(w => w.Transactions)
                .Where(w => w.PlayerId == playerId)
                .SingleOrDefaultAsync(cancellationToken);
    }

    internal record CreditTransactionDatabaseFacade(DatabaseContext DbContext) : ICreditTransactionDatabaseFacade;
}
