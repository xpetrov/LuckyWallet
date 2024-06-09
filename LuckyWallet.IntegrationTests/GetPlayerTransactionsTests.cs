using LuckyWallet.Controllers.Models;
using LuckyWallet.Host;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http.Json;

namespace LuckyWallet.IntegrationTests;

[TestClass]
public class GetPlayerTransactionsTests
{
    [TestMethod]
    public async Task GetPlayerTransactions_WhenPlayerHasTransactions_ReturnsTransactions()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();

        // act
        var response = await client.GetAsync($"api/Player/{DbDefaults.Player1_Id}/Transactions");

        // assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var transactions = await response.Content.ReadFromJsonAsync<GetTransactionModel[]>();
        Assert.IsNotNull( transactions );
        Assert.IsTrue( transactions.Length == 3 );
    }

    [TestMethod]
    public async Task GetPlayerTransactions_WhenPlayerHasNoTransactions_ReturnsEmptyTransactions()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();

        // act
        var response = await client.GetAsync($"api/Player/{DbDefaults.Player3_Id}/Transactions");

        // assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var transactions = await response.Content.ReadFromJsonAsync<GetTransactionModel[]>();
        Assert.IsNotNull(transactions);
        Assert.IsTrue(transactions.Length == 0);
    }

    [TestMethod]
    public async Task GetPlayerTransactions_WhenPlayerHasNoWallet_ReturnsEmptyTransactions()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();

        // act
        var response = await client.GetAsync($"api/Player/{DbDefaults.Player4_Id}/Transactions");

        // assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var transactions = await response.Content.ReadFromJsonAsync<GetTransactionModel[]>();
        Assert.IsNotNull(transactions);
        Assert.IsTrue(transactions.Length == 0);
    }

    [TestMethod]
    public async Task GetPlayerTransactions_WhenPlayerDoesNotExist_Returns404()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();

        // act
        var response = await client.GetAsync($"api/Player/{DbDefaults.PlayerUnknown_Id}/Transactions");

        // assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }
}
