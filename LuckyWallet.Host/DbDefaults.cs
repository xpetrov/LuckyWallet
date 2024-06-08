namespace LuckyWallet.Host;

public static class DbDefaults
{
    public static Guid Player1_Id = GuidGenerator.FromString("player-1"); // caedc976-5a7e-867f-36b9-c90bbbb6369d
    public static Guid Player2_Id = GuidGenerator.FromString("player-2"); // 41b4b564-737d-4291-5707-75e1cc4141d9
    public static Guid Player3_Id = GuidGenerator.FromString("player-3"); // b6ca9df7-ea61-664b-9565-585089d9855c
    public static Guid Player4_Id = GuidGenerator.FromString("player-4"); // 557d3646-7237-ad4b-cdb9-daedc3b95318

    public static Guid Wallet1_Id = GuidGenerator.FromString("wallet-1");
    public static Guid Wallet2_Id = GuidGenerator.FromString("wallet-2");
    public static Guid Wallet3_Id = GuidGenerator.FromString("wallet-3");

    public static Guid Wallet1_Transaction1_Id = GuidGenerator.FromString("w1-transaction-1");
    public static Guid Wallet1_Transaction2_Id = GuidGenerator.FromString("w1-transaction-2");
    public static Guid Wallet1_Transaction3_Id = GuidGenerator.FromString("w1-transaction-3");

    public static Guid Wallet2_Transaction1_Id = GuidGenerator.FromString("w2-transaction-1");
}
