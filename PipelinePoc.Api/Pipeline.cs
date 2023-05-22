using PipelinePoc.Api.Handlers;

namespace PipelinePoc.Api;

public class Pipeline
{
    private readonly ILogger<Pipeline> _logger;

    public Pipeline(ILogger<Pipeline> logger)
    {
        _logger = logger;
    }

    public async Task<IResult> Pipe<TRequest, TResponse>(
        Func<TRequest, Task<HandlerResult<TResponse>>> handler,
        TRequest request,
        string actionName)
    {
        _logger.LogInformation("Action {Action} started", actionName);
        var handlerResult = await handler(request);
        _logger.LogInformation("Action {Action} finished", actionName);

        return handlerResult.Status switch
        {
            HandlerResultStatus.Ok => Results.Ok(handlerResult.Object),
            HandlerResultStatus.NotFound => Results.NotFound(handlerResult.Object),
            HandlerResultStatus.ValidationError => Results.BadRequest(handlerResult.Errors),
            _ =>
                throw new NotImplementedException($"Cannot handle status of {handlerResult.Status}")
        };
    }
}
