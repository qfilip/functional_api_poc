using FakeItEasy;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using PipelinePoc.Api;
using PipelinePoc.Api.DataAccess;

using Endpoint = PipelinePoc.Api.Endpoints.V1_0.Item.CreateItem;
using Handler = PipelinePoc.Api.Handlers.Item.CreateItem.Handler;

namespace PipelinePoc.Tests.Endpoints.V1_0.Item;

public class CreateItemTests
{
    private readonly IItemStore _fItemStore;
    private readonly Pipeline _pipeline;
    private readonly Handler _handler;

    public CreateItemTests()
    {
        var fPipelineLogger = A.Fake<ILogger<Pipeline>>();
        var fHandlerLogger = A.Fake<ILogger<Handler>>();
        
        _fItemStore = A.Fake<IItemStore>();
        _pipeline = new Pipeline(fPipelineLogger);
        _handler = new Handler(fHandlerLogger, _fItemStore);
    }

    [Fact]
    public async Task ValidInput_Returns_Ok()
    {
        var input = "test";
        var expected = "test";

        var iResult = await Endpoint.Action(input, _pipeline, _handler);
        var result = iResult as Ok<string>;

        Assert.NotNull(result);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public async Task InvalidInput_Returns_BadRequest()
    {
        var input = "";

        var iResult = await Endpoint.Action(input, _pipeline, _handler);
        var result = iResult as BadRequest<List<string>>;

        Assert.NotNull(result);
        Assert.True(result.Value!.Count > 0);
    }

    [Fact]
    public async Task ItemExists_Returns_Exception()
    {
        var input = "test";
        
        A.CallTo(() => _fItemStore.AddAsync(A<string>.That.Matches(x => x == input)))
            .Throws<InvalidOperationException>();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            Endpoint.Action(input, _pipeline, _handler));
    }
}
