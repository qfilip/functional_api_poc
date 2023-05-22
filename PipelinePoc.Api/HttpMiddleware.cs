namespace PipelinePoc.Api;

public class HttpMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpMiddleware> _logger;

    public HttpMiddleware(RequestDelegate next, ILogger<HttpMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint()?.DisplayName;
        try
        {
            _logger.LogInformation("Endpoint {Endpoint} started", endpoint);
            await _next(context);
            _logger.LogInformation("Endpoint {Endpoint} finished", endpoint);
        }
        catch (Exception ex)
        {
            _logger.LogError("Endpoint {Endpoint} failed, reason {Reason}", endpoint, ex.Message);
            await context.Response.WriteAsync("Oops");
        }
    }
}
