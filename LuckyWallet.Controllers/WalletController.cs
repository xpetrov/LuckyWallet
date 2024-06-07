using LuckyWallet.Controllers.Infrastructure;
using LuckyWallet.Controllers.Models;
using LuckyWallet.Controllers.Operations;
using Microsoft.AspNetCore.Mvc;

namespace LuckyWallet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalletController : Controller
{
    [HttpPost(Name = nameof(RegisterWallet))]
    public async Task<ActionResult<None>> RegisterWallet(
        [FromBody] RegisterWalletModel model,
        [FromServices] RegisterWalletOperation operation,
        CancellationToken cancellationToken)
    {
        var result = await operation.Execute(model, User, cancellationToken);
        return this.OperationResult(result);
    }

    [HttpGet("Balance/{playerId}", Name = nameof(GetPlayerBalance))]
    public async Task<ActionResult<decimal>> GetPlayerBalance(
        [FromRoute] Guid playerId,
        [FromServices] GetPlayerBalanceOperation operation,
        CancellationToken cancellationToken)
    {
        var result = await operation.Execute(playerId, User, cancellationToken);
        return this.OperationResult(result);
    }
}
