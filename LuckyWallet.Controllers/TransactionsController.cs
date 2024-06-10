using LuckyWallet.Controllers.Infrastructure;
using LuckyWallet.Controllers.Models;
using LuckyWallet.Controllers.Operations;
using Microsoft.AspNetCore.Mvc;

namespace LuckyWallet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : Controller
{
    [HttpPost(Name = nameof(CreditTransaction))]
    public async Task<ActionResult<None>> CreditTransaction(
        [FromBody] CreditTransactionModel model,
        [FromServices] CreditTransactionOperation operation,
        CancellationToken cancellationToken)
    {
        var result = await operation.Execute(model, User, cancellationToken);
        return this.OperationResult(result);
    }
}
