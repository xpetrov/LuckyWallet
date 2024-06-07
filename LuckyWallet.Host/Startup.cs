using LuckyWallet.Controllers;
using LuckyWallet.DataAccess;
using LuckyWallet.DataModel;
using LuckyWallet.DataModel.Enums;
using Microsoft.EntityFrameworkCore;

namespace LuckyWallet.Host;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("LuckyWalletDb"));

        services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddWalletOperations();

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        SeedData(dbContext);

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private static void SeedData(DatabaseContext dbContext)
    {
        var defaultWallets = new List<Wallet>()
        {
            new() { Id = DbDefaults.Wallet1_Id, PlayerId = DbDefaults.Player1_Id, Balance = 100 },
            new() { Id = DbDefaults.Wallet2_Id, PlayerId = DbDefaults.Player2_Id, Balance = 50 },
            new() { Id = DbDefaults.Wallet3_Id, PlayerId = DbDefaults.Player3_Id, Balance = 0 }
        };

        var defaultTransactions = new List<Transaction>()
        {
            new() { Id = DbDefaults.Wallet1_Transaction1_Id, UniqueTransactionId = Guid.NewGuid(), WalletId = DbDefaults.Wallet1_Id, Amount = 50, Type = TransactionType.Deposit },
            new() { Id = DbDefaults.Wallet1_Transaction2_Id, UniqueTransactionId = Guid.NewGuid(), WalletId = DbDefaults.Wallet1_Id, Amount = 75, Type = TransactionType.Win },
            new() { Id = DbDefaults.Wallet1_Transaction3_Id, UniqueTransactionId = Guid.NewGuid(), WalletId = DbDefaults.Wallet1_Id, Amount = 25, Type = TransactionType.Stake },

            new() { Id = DbDefaults.Wallet2_Transaction1_Id, UniqueTransactionId = Guid.NewGuid(), WalletId = DbDefaults.Wallet2_Id, Amount = 50, Type = TransactionType.Deposit }
        };

        dbContext.Wallets.AddRange(defaultWallets);
        dbContext.Transactions.AddRange(defaultTransactions);
        dbContext.SaveChanges();
    }
}
