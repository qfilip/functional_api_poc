using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using PipelinePoc.Api;

namespace PipelinePoc.Tests;

public class StartupTests
{
    [Fact]
    public void ServicesScopes_AreValid()
    {
        var builder = WebApplication.CreateBuilder();

        ServiceRegistry.AddAppServices(builder);

        builder.Host.UseDefaultServiceProvider((_, options) =>
        {
            options.ValidateOnBuild = true;
            options.ValidateScopes = true;
        });

        builder.Build();
    }
}
