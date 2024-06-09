using LuckyWallet.Controllers.Operations;
using Microsoft.Extensions.DependencyInjection;

namespace LuckyWallet.Controllers;

public static class Registrations
{
    public static IServiceCollection AddOperations(this IServiceCollection services)
    {
        RegisterWalletOperation.Register(services);
        GetPlayerBalanceOperation.Register(services);
        GetPlayerTransactionsOperation.Register(services);
        CreditTransactionOperation.Register(services);

        return services;
    }
}
