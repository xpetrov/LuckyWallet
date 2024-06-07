using System.Security.Cryptography;
using System.Text;

namespace LuckyWallet.Host;

public class GuidGenerator
{
    public static Guid FromString(string input)
    {
        byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes(input));
        return new Guid(hash);
    }
}
