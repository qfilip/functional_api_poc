﻿using FakeItEasy;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using PipelinePoc.Api;
using PipelinePoc.Api.DataAccess;

using Endpoint = PipelinePoc.Api.Endpoints.V1_0.Item.GetAllItems;
using Handler = PipelinePoc.Api.Handlers.Item.GetAllItems.Handler;

namespace PipelinePoc.Tests.Endpoints.V1_0.Item;

public class GetAllItemsTests
{
    private readonly IItemStore _fItemStore;
    private readonly Pipeline _pipeline;
    private readonly Handler _handler;

    public GetAllItemsTests()
    {
        var fPipelineLogger = A.Fake<ILogger<Pipeline>>();

        _fItemStore = A.Fake<IItemStore>();
        _pipeline = new Pipeline(fPipelineLogger);
        _handler = new Handler(_fItemStore);
    }

    [Fact]
    public async Task OnCall_Returns_Ok()
    {
        var fResult = new List<string> { "fnaah", "gauguin" };
        A.CallTo(() => _fItemStore.GetAllAsync())
            .Returns(fResult);

        var iResult = await Endpoint.Action(_pipeline, _handler);
        var result = iResult as Ok<IEnumerable<string>>;

        Assert.NotNull(result);
        Assert.True(result.Value!.Count() == fResult.Count());
    }
}
