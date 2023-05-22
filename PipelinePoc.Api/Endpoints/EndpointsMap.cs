using PipelinePoc.Api.Endpoints.V1_0;

namespace PipelinePoc.Api.Endpoints;

public static class EndpointsMap
{
    public static void Map(WebApplication app)
    {
        V1_0Endpoints.Map(app);
    }
}
