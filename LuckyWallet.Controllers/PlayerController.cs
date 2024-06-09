using LuckyWallet.Controllers.Infrastructure;
using LuckyWallet.Controllers.Mappings;
using LuckyWallet.Controllers.Models;
using LuckyWallet.Controllers.Operations;
using Microsoft.AspNetCore.Mvc;

namespace LuckyWallet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : Controller
{
    [HttpPost("{playerId}/RegisterWallet", Name = nameof(RegisterWallet))]
    public async Task<ActionResult<None>> RegisterWallet(
        [FromRoute] Guid playerId,
        [FromServices] RegisterWalletOperation operation,
        CancellationToken cancellationToken)
    {
        var result = await operation.Execute(playerId, User, cancellationToken);
        return this.OperationResult(result);
    }

    [HttpGet("{playerId}/Balance", Name = nameof(GetPlayerBalance))]
    public async Task<ActionResult<decimal>> GetPlayerBalance(
        [FromRoute] Guid playerId,
        [FromServices] GetPlayerBalanceOperation operation,
        CancellationToken cancellationToken)
    {
        var result = await operation.Execute(playerId, User, cancellationToken);
        return this.OperationResult(result);
    }

    [HttpGet("{playerId}/Transactions", Name = nameof(GetPlayerTransactions))]
    public async Task<ActionResult<GetTransactionModel[]>> GetPlayerTransactions(
        [FromRoute] Guid playerId,
        [FromServices] GetPlayerTransactionsOperation operation,
        CancellationToken cancellationToken)
    {
        var result = await operation.Execute(playerId, User, cancellationToken);
        return this.OperationResult(result, res => res.Select(_ => TransactionMappings.ToGetPlayerTransactionModel(_)).ToArray());
    }
}
