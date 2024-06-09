using LuckyWallet.Controllers.Infrastructure;
using LuckyWallet.DataAccess;
using LuckyWallet.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Security.Claims;

namespace LuckyWallet.Controllers.Operations;

public class GetPlayerTransactionsOperation : OperationBase<Guid, Transaction[]>
{
    private readonly IGetPlayerTransactionsDatabaseFacade _facade;

    public GetPlayerTransactionsOperation(IGetPlayerTransactionsDatabaseFacade facade) =>
        _facade = facade;

    public static IServiceCollection Register(IServiceCollection services) =>
        services
            .AddScoped<GetPlayerTransactionsOperation>()
            .AddScoped<IGetPlayerTransactionsDatabaseFacade, GetPlayerTransactionsDatabaseFacade>();

    protected override async Task<Transaction[]> ExecuteCore(
        Guid input,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken)
    {
        await Validate(input, cancellationToken);

        return await _facade.GetTransactions(input, cancellationToken);
    }

    private async Task Validate(Guid input, CancellationToken cancellationToken)
    {
        if (!await _facade.PlayerExists(input, cancellationToken))
        {
            throw new OperationErrorException(HttpStatusCode.NotFound, "Player Not Found.");
        }
    }

    public interface IGetPlayerTransactionsDatabaseFacade : IRequiresContext
    {
        Task<bool> PlayerExists(Guid playerId, CancellationToken cancellationToken) =>
            DbContext.Set<Player>().AnyAsync(_ => _.Id == playerId, cancellationToken);

        Task<bool> PlayerHasWallet(Guid playerId, CancellationToken cancellationToken) =>
            DbContext.Set<Wallet>().AnyAsync(_ => _.PlayerId == playerId, cancellationToken);

        Task<Transaction[]> GetTransactions(Guid playerId, CancellationToken cancellationToken) =>
            DbContext.Set<Transaction>()
                .AsNoTracking()
                .Where(_ => _.Wallet.PlayerId == playerId)
                .ToArrayAsync(cancellationToken);
    }

    internal record GetPlayerTransactionsDatabaseFacade(DatabaseContext DbContext) : IGetPlayerTransactionsDatabaseFacade;
}
