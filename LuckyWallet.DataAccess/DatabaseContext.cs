using LuckyWallet.DataModel;
using Microsoft.EntityFrameworkCore;

namespace LuckyWallet.DataAccess;

public class DatabaseContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Player>()
            .HasOne(p => p.Wallet)
            .WithOne(w => w.Player)
            .HasForeignKey<Wallet>(w => w.PlayerId)
            .IsRequired();

        //modelBuilder.Entity<Wallet>().Property(_ => _.Balance).HasPrecision(18, 2);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Wallet)
            .WithMany(w => w.Transactions)
            .HasForeignKey(t => t.WalletId)
            .IsRequired();
        //modelBuilder.Entity<Transaction>().Property(_ => _.Amount).HasPrecision(18, 2);
    }
}