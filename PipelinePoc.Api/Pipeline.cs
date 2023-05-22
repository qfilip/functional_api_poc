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

        if (handlerResult.Status == HandlerResultStatus.Ok)
        {
            return Results.Ok(handlerResult.Object);
        }
        else if (handlerResult.Status == HandlerResultStatus.NotFound)
        {
            return Results.NotFound(handlerResult.Object);
        }
        else if (handlerResult.Status == HandlerResultStatus.ValidationError)
        {
            return Results.BadRequest(handlerResult.Errors);
        }
        else
        {
            var status = handlerResult.Status.ToString();
            throw new NotImplementedException($"Cannot handle status of {status}");
        }
    }
}
