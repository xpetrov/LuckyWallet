using LuckyWallet.Controllers.Infrastructure;
using LuckyWallet.Controllers.Models;
using LuckyWallet.DataAccess;
using LuckyWallet.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace LuckyWallet.Controllers.Operations;

public class RegisterWalletOperation : OperationBase<RegisterWalletModel>
{
    private readonly IRegisterWalletDatabaseFacade _facade;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterWalletOperation(IRegisterWalletDatabaseFacade facade, IUnitOfWork unitOfWork)
    {
        _facade = facade;
        _unitOfWork = unitOfWork;
    }

    public static IServiceCollection Register(IServiceCollection services) =>
        services
            .AddScoped<RegisterWalletOperation>()
            .AddScoped<IRegisterWalletDatabaseFacade, RegisterWalletDatabaseFacade>();

    protected override async Task<None> ExecuteCore(
        RegisterWalletModel input,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken)
    {
        var wallet = new Wallet()
        {
            Balance = 0,
            PlayerId = input.PlayerId
        };

        if (await _facade.PlayerHasWallet(input.PlayerId, cancellationToken))
        {
            throw new OperationErrorException($"Player's Wallet Already Registered - PlayerId:{input.PlayerId}");
        }

        _facade.CreateWallet(wallet);
        await _unitOfWork.SaveAsync(cancellationToken);

        return None.Value;
    }

    public interface IRegisterWalletDatabaseFacade : IRequiresContext
    {
        Task<bool> PlayerHasWallet(Guid playerId, CancellationToken cancellationToken) =>
            DbContext.Set<Wallet>()
            .AnyAsync(_ => _.PlayerId == playerId, cancellationToken);

        void CreateWallet(Wallet wallet) =>
            DbContext.Set<Wallet>().Add(wallet);
    }

    internal record RegisterWalletDatabaseFacade(DatabaseContext DbContext) : IRegisterWalletDatabaseFacade;
}
