using LuckyWallet.Controllers.Models;
using LuckyWallet.DataModel.Enums;
using LuckyWallet.Host;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;

namespace LuckyWallet.IntegrationTests;

[TestClass]
public class CreditTransactionTests : ApiFacade
{
    [TestMethod]
    public async Task CreditTransaction_WhenDeposit_ReturnsSuccess()
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
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/Transactions");
        AddJsonContent(request, model);

        // act
        var response = await client.SendAsync(request);

        // assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task CreditTransaction_WhenDepositRepeatedly_ReturnsSuccess()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();
        var model = new CreditTransactionModel
        (
            UniqueTransactionId: Guid.NewGuid(),
            PlayerId: DbDefaults.Player1_Id,
            Amount: 300,
            Type: TransactionType.Deposit
        );
        var request1 = new HttpRequestMessage(HttpMethod.Post, $"api/Transactions");
        AddJsonContent(request1, model);
        var request2 = new HttpRequestMessage(HttpMethod.Post, $"api/Transactions");
        AddJsonContent(request2, model);

        // act
        var response1 = await client.SendAsync(request1);
        var response2 = await client.SendAsync(request2);

        // assert
        Assert.AreEqual(HttpStatusCode.OK, response1.StatusCode);
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);

        var response3 = await client.GetAsync($"api/Players/{DbDefaults.Player1_Id}/Balance");
        Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);

        var responseString = await response3.Content.ReadAsStringAsync();
        var balance = JsonConvert.DeserializeObject<decimal>(responseString);
        Assert.AreEqual(400, balance);
    }

    [TestMethod]
    public async Task CreditTransaction_WhenWin_ReturnsSuccess()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();
        var model = new CreditTransactionModel
        (
            UniqueTransactionId: Guid.NewGuid(),
            PlayerId: DbDefaults.Player3_Id,
            Amount: 300,
            Type: TransactionType.Win
        );
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/Transactions");
        AddJsonContent(request, model);

        // act
        var response = await client.SendAsync(request);

        // assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task CreditTransaction_WhenWinRepeatedly_ReturnsSuccess()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();
        var model = new CreditTransactionModel
        (
            UniqueTransactionId: Guid.NewGuid(),
            PlayerId: DbDefaults.Player1_Id,
            Amount: 300,
            Type: TransactionType.Win
        );
        var request1 = new HttpRequestMessage(HttpMethod.Post, $"api/Transactions");
        AddJsonContent(request1, model);
        var request2 = new HttpRequestMessage(HttpMethod.Post, $"api/Transactions");
        AddJsonContent(request2, model);

        // act
        var response1 = await client.SendAsync(request1);
        var response2 = await client.SendAsync(request2);

        // assert
        Assert.AreEqual(HttpStatusCode.OK, response1.StatusCode);
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);

        var response3 = await client.GetAsync($"api/Players/{DbDefaults.Player1_Id}/Balance");
        Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);

        var responseString = await response3.Content.ReadAsStringAsync();
        var balance = JsonConvert.DeserializeObject<decimal>(responseString);
        Assert.AreEqual(400, balance);
    }

    [TestMethod]
    public async Task CreditTransaction_WhenStakeWithSufficientBalance_ReturnsSuccess()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();
        var model = new CreditTransactionModel
        (
            UniqueTransactionId: Guid.NewGuid(),
            PlayerId: DbDefaults.Player1_Id,
            Amount: 50,
            Type: TransactionType.Stake
        );
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/Transactions");
        AddJsonContent(request, model);

        // act
        var response = await client.SendAsync(request);

        // assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var response2 = await client.GetAsync($"api/Players/{DbDefaults.Player1_Id}/Balance");
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);

        var responseString = await response2.Content.ReadAsStringAsync();
        var balance = JsonConvert.DeserializeObject<decimal>(responseString);
        Assert.AreEqual(50, balance);
    }

    [TestMethod]
    public async Task CreditTransaction_WhenStakeWithInsufficientBalance_ReturnsRejected()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();
        var model = new CreditTransactionModel
        (
            UniqueTransactionId: Guid.NewGuid(),
            PlayerId: DbDefaults.Player1_Id,
            Amount: 500,
            Type: TransactionType.Stake
        );
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/Transactions");
        AddJsonContent(request, model);

        // act
        var response = await client.SendAsync(request);

        // assert
        Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("Rejected due to insufficient funds.", responseString);

        var response2 = await client.GetAsync($"api/Players/{DbDefaults.Player1_Id}/Balance");
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);

        responseString = await response2.Content.ReadAsStringAsync();
        var balance = JsonConvert.DeserializeObject<decimal>(responseString);
        Assert.AreEqual(100, balance);
    }

    [TestMethod]
    public async Task CreditTransaction_WhenStakeWithInsufficientBalanceRepeatedly_ReturnsRejected()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();
        var model = new CreditTransactionModel
        (
            UniqueTransactionId: Guid.NewGuid(),
            PlayerId: DbDefaults.Player1_Id,
            Amount: 500,
            Type: TransactionType.Stake
        );
        var request1 = new HttpRequestMessage(HttpMethod.Post, $"api/Transactions");
        AddJsonContent(request1, model);
        var request2 = new HttpRequestMessage(HttpMethod.Post, $"api/Transactions");
        AddJsonContent(request2, model);

        // act
        var response1 = await client.SendAsync(request1);
        var response2 = await client.SendAsync(request2);

        // assert
        Assert.AreEqual(HttpStatusCode.Conflict, response1.StatusCode);
        Assert.AreEqual(HttpStatusCode.Conflict, response2.StatusCode);

        var response3 = await client.GetAsync($"api/Players/{DbDefaults.Player1_Id}/Balance");
        Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);

        var responseString = await response3.Content.ReadAsStringAsync();
        var balance = JsonConvert.DeserializeObject<decimal>(responseString);
        Assert.AreEqual(100, balance);
    }
}
