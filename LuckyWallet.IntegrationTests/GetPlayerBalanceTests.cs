using LuckyWallet.Host;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;

namespace LuckyWallet.IntegrationTests;

[TestClass]
public class GetPlayerBalanceTests
{
    [TestMethod]
    public async Task GetPlayerBalance_WhenPlayerHasPositiveBalance_ReturnsCorrectAmount()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();

        // act
        var response = await client.GetAsync($"api/Player/{DbDefaults.Player1_Id}/Balance");

        // assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var balance = JsonConvert.DeserializeObject<decimal>(responseString);
        Assert.AreEqual(100, balance);
    }

    [TestMethod]
    public async Task GetPlayerBalance_WhenPlayerHasZeroBalance_ReturnsZero()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();

        // act
        var response = await client.GetAsync($"api/Player/{DbDefaults.Player3_Id}/Balance");

        // assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var balance = JsonConvert.DeserializeObject<decimal>(responseString);
        Assert.AreEqual(0, balance);
    }

    [TestMethod]
    public async Task GetPlayerBalance_WhenPlayerHasNoWallet_Returns404()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();

        // act
        var response = await client.GetAsync($"api/Player/{DbDefaults.Player4_Id}/Balance");

        // assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("Wallet Not Found.", responseString);
    }
}