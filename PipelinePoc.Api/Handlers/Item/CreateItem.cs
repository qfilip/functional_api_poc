using PipelinePoc.Api.DataAccess;

namespace PipelinePoc.Api.Handlers.Item;

public static class CreateItem
{
    public class Handler : IRequestHandler
    {
        private readonly ILogger<Handler> _logger;
        private readonly IItemStore _store;
        public Handler(ILogger<Handler> logger, IItemStore store)
        {
            _logger = logger;
            _store = store;
        }

        public async Task<HandlerResult<string>> Handle(string? item)
        {
            var handlerResult = Validate(item);
            if(handlerResult.Status != HandlerResultStatus.Ok)
            {
                _logger.LogInformation("Item {Item} not found", handlerResult.Object);
                return handlerResult;
            }

            await _store.AddAsync(handlerResult.Object!);
            
            return handlerResult;
        }
    }

    public static HandlerResult<string> Validate(string? item)
    {
        if(string.IsNullOrEmpty(item))
        {
            return HandlerResult<string>.ValidationError("Item is empty");
        }

        return HandlerResult<string>.Ok(item);
    }
}
