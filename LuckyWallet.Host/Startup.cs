using LuckyWallet.Controllers;
using LuckyWallet.DataAccess;
using LuckyWallet.DataModel;
using LuckyWallet.DataModel.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace LuckyWallet.Host;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseInMemoryDatabase("LuckyWalletDb");
        });

        services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddOperations();

        services.AddControllers();

        services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "LuckyWallet API", Version = "v1" }));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        SeedData(dbContext);

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LuckyWallet API V1"));
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private static void SeedData(DatabaseContext dbContext)
    {
        var defaultPlayers = new List<Player>()
        {
            new() { Id = DbDefaults.Player1_Id },
            new() { Id = DbDefaults.Player2_Id },
            new() { Id = DbDefaults.Player3_Id },
            new() { Id = DbDefaults.Player4_Id }
        };
        dbContext.Players.AddRange(defaultPlayers);
        dbContext.SaveChanges();

        var defaultWallets = new List<Wallet>()
        {
            /*Player 1*/ new() { Id = DbDefaults.Wallet1_Id, PlayerId = DbDefaults.Player1_Id, Balance = 100 },
            /*Player 2*/ new() { Id = DbDefaults.Wallet2_Id, PlayerId = DbDefaults.Player2_Id, Balance = 50 },
            /*Player 3*/ new() { Id = DbDefaults.Wallet3_Id, PlayerId = DbDefaults.Player3_Id, Balance = 0 },
            /*Player 4 has no wallet registered.*/
        };
        dbContext.Wallets.AddRange(defaultWallets);
        dbContext.SaveChanges();

        var defaultTransactions = new List<Transaction>()
        {
            new()
            {
                Id = DbDefaults.Wallet1_Transaction1_Id,
                UniqueTransactionId = DbDefaults.Wallet1_Transaction1_UniqueId,
                WalletId = DbDefaults.Wallet1_Id,
                Amount = 50,
                Type = TransactionType.Deposit,
                Result = TransactionResult.Accepted
            },
            new()
            {
                Id = DbDefaults.Wallet1_Transaction2_Id,
                UniqueTransactionId = DbDefaults.Wallet1_Transaction2_UniqueId,
                WalletId = DbDefaults.Wallet1_Id,
                Amount = 75,
                Type = TransactionType.Win,
                Result = TransactionResult.Accepted
            },
            new()
            {
                Id = DbDefaults.Wallet1_Transaction3_Id,
                UniqueTransactionId = DbDefaults.Wallet1_Transaction3_UniqueId,
                WalletId = DbDefaults.Wallet1_Id,
                Amount = 25,
                Type = TransactionType.Stake,
                Result = TransactionResult.Accepted
            },
            new()
            {
                Id = DbDefaults.Wallet2_Transaction1_Id,
                UniqueTransactionId = DbDefaults.Wallet2_Transaction1_UniqueId,
                WalletId = DbDefaults.Wallet2_Id,
                Amount = 50,
                Type = TransactionType.Deposit,
                Result = TransactionResult.Accepted
            }
        };
        dbContext.Transactions.AddRange(defaultTransactions);
        dbContext.SaveChanges();
    }
}
