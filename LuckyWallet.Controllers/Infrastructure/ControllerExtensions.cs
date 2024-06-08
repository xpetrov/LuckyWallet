using Microsoft.AspNetCore.Mvc;

namespace LuckyWallet.Controllers.Infrastructure;

public static class ControllerExtensions
{
    public static ActionResult<T> OperationResult<T>(
    this Controller controller,
    OperationResult<T> result)
    {
        return result switch
        {
            Success<T> { Value: None or null } => controller.Ok(),
            Success<T> success => controller.Ok(success.Value),
            OperationError<T> operationError => controller.StatusCode((int)operationError.StatusCode, operationError.Message),
            _ => throw new NotSupportedException()
        };
    }

    public static ActionResult<TOutput> OperationResult<TInput, TOutput>(
        this Controller controller,
        OperationResult<TInput> result,
        Func<TInput, TOutput> map)
    {
        return result switch
        {
            Success<TInput> { Value: None or null } => controller.Ok(),
            Success<TInput> success => controller.Ok(map(success.Value)),
            OperationError<TInput> operationError => controller.StatusCode((int)operationError.StatusCode, operationError.Message),
            _ => throw new NotSupportedException()
        };
    }

    public static async Task<ActionResult<T>> OperationResult<T>(
        this Controller controller,
        Task<OperationResult<T>> resultTask) =>
        OperationResult(controller, await resultTask);

    public static async Task<ActionResult<TOutput>> OperationResult<TInput, TOutput>(
        this Controller controller,
        Task<OperationResult<TInput>> resultTask,
        Func<TInput, TOutput> map) =>
        OperationResult(controller, await resultTask, map);
}
