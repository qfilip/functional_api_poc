namespace PipelinePoc.Api.Endpoints.V1_0.Item;

public static class GetAllItems
{
    public static async Task<IResult> Action(
        Pipeline pipeline,
        Handlers.Item.GetAllItems.Handler handler)
    {
        return await pipeline.Pipe(handler.Handle, new Unit(), nameof(Handlers.Item.GetAllItems));
    }
}
