using FakeItEasy;
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
