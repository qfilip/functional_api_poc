using PipelinePoc.Api.Handlers.Item;

namespace PipelinePoc.Api.Endpoints.V1_0.Item;

public static class GetAllEndpoint
{
    public static async Task<IResult> Action(
        Pipeline pipeline,
        GetItems.Handler handler)
    {
        return await pipeline.Pipe(handler.Handle, new Unit(), nameof(GetItems));
    }
}
