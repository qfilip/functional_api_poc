using Microsoft.AspNetCore.Mvc;
using PipelinePoc.Api.Handlers.Item;

namespace PipelinePoc.Api.Endpoints.V1_0.Item;

public static class CreateEndpoint
{
    public static async Task<IResult> Action(
        [FromBody]string item,
        Pipeline pipeline,
        CreateItem.Handler handler)
    {
        return await pipeline.Pipe(handler.Handle, item, nameof(CreateItem));
    }
}
