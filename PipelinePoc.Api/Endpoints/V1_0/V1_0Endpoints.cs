namespace PipelinePoc.Api.Endpoints.V1_0;

public static class V1_0Endpoints
{
    public static void Map(WebApplication app)
    {
        var items = app.MapGroup("/v1/items");
        
        items.MapPost("/create", Item.CreateEndpoint.Action);
        items.MapGet("/getall", Item.GetAllEndpoint.Action);
    }
}
