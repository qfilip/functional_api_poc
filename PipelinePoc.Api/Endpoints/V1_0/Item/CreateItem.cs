using Microsoft.AspNetCore.Mvc;

namespace PipelinePoc.Api.Endpoints.V1_0.Item;

public static class CreateItem
{
    public static async Task<IResult> Action(
        [FromBody]string item,
        Pipeline pipeline,
        Handlers.Item.CreateItem.Handler handler)
    {
        return await pipeline.Pipe(handler.Handle, item, nameof(Handlers.Item.CreateItem));
    }
}
