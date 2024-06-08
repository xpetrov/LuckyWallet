namespace LuckyWallet.DataModel;

public class Wallet
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }

    public Player Player { get; set; } = null!;

    public decimal Balance { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = null!;
}