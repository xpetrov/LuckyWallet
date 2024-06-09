using System.Text.Json;
using System.Text;

namespace LuckyWallet.IntegrationTests;

public abstract class ApiFacade
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new();

    public void AddJsonContent(HttpRequestMessage request, object? data)
    {
        if (data is null)
        {
            return;
        }

        var content = JsonSerializer.Serialize(data, _jsonSerializerOptions);
        request.Content = new StringContent(content, Encoding.UTF8, "application/json");
    }
}
