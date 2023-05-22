using PipelinePoc.Api.DataAccess;

namespace PipelinePoc.Api.Handlers.Item;

public class GetAllItems
{
    public class Handler : IRequestHandler
    {
        private readonly IItemStore _store;
        public Handler(IItemStore store)
        {
            _store = store;
        }

        public async Task<HandlerResult<IEnumerable<string>>> Handle(Unit unit)
        {
            var items = await _store.GetAllAsync();
            return HandlerResult<IEnumerable<string>>.Ok(items);
        }
    }
}
