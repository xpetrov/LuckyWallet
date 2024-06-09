using LuckyWallet.Controllers.Models;
using LuckyWallet.DataModel.Enums;
using LuckyWallet.Host;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace LuckyWallet.IntegrationTests;

[TestClass]
public class CreditTransactionTests : ApiFacade
{
    [TestMethod]
    public async Task CreditTransaction_WhenPlayerDeposit_ReturnsSuccess()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();
        var model = new CreditTransactionModel
        (
            UniqueTransactionId: Guid.NewGuid(),
            PlayerId: DbDefaults.Player3_Id,
            Amount: 300,
            Type: TransactionType.Deposit
        );
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/Transaction");
        AddJsonContent(request, model);

        // act
        var response = await client.SendAsync(request);

        // assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
