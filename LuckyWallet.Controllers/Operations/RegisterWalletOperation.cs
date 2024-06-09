using LuckyWallet.Controllers.Infrastructure;
using LuckyWallet.DataAccess;
using LuckyWallet.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Security.Claims;

namespace LuckyWallet.Controllers.Operations;

public class RegisterWalletOperation : OperationBase<Guid>
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
        Guid input,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken)
    {
        await Validate(input, cancellationToken);

        var wallet = new Wallet()
        {
            Id = Guid.NewGuid(),
            Balance = 0,
            PlayerId = input
        };

        _facade.CreateWallet(wallet);
        await _unitOfWork.SaveAsync(cancellationToken);

        return None.Value;
    }

    private async Task Validate(Guid input, CancellationToken cancellationToken)
    {
        if (!await _facade.PlayerExists(input, cancellationToken))
        {
            throw new OperationErrorException(HttpStatusCode.NotFound, "Player Not Found.");
        }

        if (await _facade.PlayerHasWallet(input, cancellationToken))
        {
            throw new OperationErrorException(HttpStatusCode.Conflict, "Player's Wallet Already Registered.");
        }
    }

    public interface IRegisterWalletDatabaseFacade : IRequiresContext
    {
        Task<bool> PlayerExists(Guid playerId, CancellationToken cancellationToken) =>
            DbContext.Set<Player>().AnyAsync(_ => _.Id == playerId, cancellationToken);

        Task<bool> PlayerHasWallet(Guid playerId, CancellationToken cancellationToken) =>
            DbContext.Set<Wallet>().AnyAsync(_ => _.PlayerId == playerId, cancellationToken);

        void CreateWallet(Wallet wallet) =>
            DbContext.Set<Wallet>().Add(wallet);
    }

    internal record RegisterWalletDatabaseFacade(DatabaseContext DbContext) : IRegisterWalletDatabaseFacade;
}
