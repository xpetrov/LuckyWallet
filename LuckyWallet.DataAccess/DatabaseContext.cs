using LuckyWallet.DataModel;
using Microsoft.EntityFrameworkCore;

namespace LuckyWallet.DataAccess;

public class DatabaseContext : DbContext
{
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Wallet>().Property(_ => _.Balance).HasPrecision(18, 2);

        modelBuilder.Entity<Transaction>().HasOne(_ => _.Wallet).WithMany(_ => _.Transactions).HasForeignKey(_ => _.WalletId);
        modelBuilder.Entity<Transaction>().Property(_ => _.Amount).HasPrecision(18, 2);
    }
}
