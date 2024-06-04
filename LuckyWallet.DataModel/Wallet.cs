namespace LuckyWallet.DataModel;

public class Wallet
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }

    public decimal Balance { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = null!;
}