using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using PipelinePoc.Api;
using PipelinePoc.Api.Handlers;

namespace PipelinePoc.Tests;

public class PipelineTests
{
    private readonly Pipeline _pipeline;

    public PipelineTests()
    {
        _pipeline = new Pipeline(A.Fake<ILogger<Pipeline>>());
    }

    [Fact]
    public async Task CustomMapper_WhenUsed_Returns_MappedResult()
    {
        var handlerResult = HandlerResult<int>.Ok(42);

        var handler = (int _) => Task.FromResult(handlerResult);
        
        var customMapper = (HandlerResult<int> hr) =>
            (hr.Status == HandlerResultStatus.Ok) ? Results.Accepted() : null;
        
        var iResult = await _pipeline.Pipe(handler, 0, "test-lambda", customMapper);

        Assert.True(iResult is Accepted);
    }

    [Fact]
    public async Task CustomMapper_WhenSkipped_Returns_PipelineMappedResult()
    {
        var handlerResult = HandlerResult<int>.NotFound();

        var handler = (int _) => Task.FromResult(handlerResult);
        
        var customMapper = (HandlerResult<int> hr) =>
            (hr.Status == HandlerResultStatus.Ok) ? Results.Accepted() : null;
        
        var iResult = await _pipeline.Pipe(handler, 0, "test-lambda", customMapper);
        
        Assert.True(iResult is NotFound<int>);
    }

    [Fact]
    public async Task AllHandlerResults_AreMappable()
    {
        var statuses = Enum.GetValues<HandlerResultStatus>();
        
        foreach(var s in statuses)
        {
            var handlerResult = HandlerResult<int>.Ok(42);
            SetPropertyValue(handlerResult, nameof(HandlerResult<int>.Status), s);
            SetPropertyValue(handlerResult, nameof(HandlerResult<int>.Errors), new List<string> { "oops" });

            var handler = (int _) => Task.FromResult(handlerResult);
            var iResult = await _pipeline.Pipe(handler, 0, "test-lambda");
            
            Assert.NotNull(iResult);
        }
    }

    private void SetPropertyValue<T, U>(T obj, string propName, U propValue)
    {
        typeof(T)
            .GetProperties()
            .FirstOrDefault(x => x.Name == propName)!
            .SetValue(obj, propValue);
    }
}
