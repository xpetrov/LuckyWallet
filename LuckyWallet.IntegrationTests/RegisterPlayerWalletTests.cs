using LuckyWallet.Host;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace LuckyWallet.IntegrationTests;

[TestClass]
public class RegisterPlayerWalletTests
{
    [TestMethod]
    public async Task RegisterPlayerWallet_WhenPlayerHasNoWallet_ReturnsSuccess()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/Player/{DbDefaults.Player4_Id}/RegisterWallet");

        // act
        var response = await client.SendAsync(request);

        // assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task RegisterPlayerWallet_PlayerDoesNotExist_Returns404()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/Player/{DbDefaults.PlayerUnknown_Id}/RegisterWallet");

        // act
        var response = await client.SendAsync(request);

        // assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task RegisterPlayerWallet_WhenPlayerAlreadyHasWallet_Returns409()
    {
        // arrange
        using var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/Player/{DbDefaults.Player1_Id}/RegisterWallet");

        // act
        var response = await client.SendAsync(request);

        // assert
        Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
    }
}
