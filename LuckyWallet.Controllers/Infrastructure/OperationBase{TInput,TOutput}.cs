using System.Security.Claims;

namespace LuckyWallet.Controllers.Infrastructure;

public abstract class OperationBase<TInput, TOutput>
{
	public async Task<OperationResult<TOutput>> Execute(
		TInput input,
		ClaimsPrincipal principal,
		CancellationToken cancellationToken)
	{
		try
		{
			var result = await ExecuteCore(input, principal, cancellationToken);
			return OperationResult.Success(result);
		}
		catch (OperationErrorException ex)
		{
			return OperationResult.OperationError<TOutput>(ex.Message);
		}
	}

	protected abstract Task<TOutput> ExecuteCore(
		TInput input,
		ClaimsPrincipal principal,
		CancellationToken cancellationToken);
}