﻿using LuckyWallet.Controllers.Infrastructure;
using LuckyWallet.DataAccess;
using LuckyWallet.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace LuckyWallet.Controllers.Operations;

public class GetPlayerBalanceOperation : OperationBase<Guid, decimal>
{
    private readonly IGetPlayerBalanceDatabaseFacade _facade;

    public GetPlayerBalanceOperation(IGetPlayerBalanceDatabaseFacade facade) => _facade = facade;

    public static IServiceCollection Register(IServiceCollection services) =>
        services
            .AddScoped<GetPlayerBalanceOperation>()
            .AddScoped<IGetPlayerBalanceDatabaseFacade, GetPlayerBalanceDatabaseFacade>();

    protected override async Task<decimal> ExecuteCore(
        Guid input,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken) =>
        (await _facade.GetPlayerBalance(input, cancellationToken))
            .ThrowIfNull("Wallet Not Found.");

    public interface IGetPlayerBalanceDatabaseFacade : IRequiresContext
    {
        Task<decimal?> GetPlayerBalance(Guid playerId, CancellationToken cancellationToken) =>
            DbContext.Set<Wallet>()
                .Where(_ => _.PlayerId == playerId)
                .Select(_ => (decimal?)_.Balance)
                .FirstOrDefaultAsync(cancellationToken);
    }

    internal record GetPlayerBalanceDatabaseFacade(DatabaseContext DbContext) : IGetPlayerBalanceDatabaseFacade;
}
