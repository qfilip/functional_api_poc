using PipelinePoc.Api.DataAccess;
using PipelinePoc.Api.Handlers;

namespace PipelinePoc.Api;

public class ServiceRegistry
{
    public static void AddAppServices(WebApplicationBuilder builder)
    {
        var handlers = typeof(Program)
        .Assembly
        .GetTypes()
        .Where(x =>
            typeof(IRequestHandler).IsAssignableFrom(x) &&
            !x.IsAbstract &&
            !x.IsInterface)
        .ToList();

        handlers.ForEach(x => builder.Services.AddTransient(x));

        builder.Services.AddScoped<Pipeline>();

        builder.Services.AddScoped<IItemStore, ItemStore>(_ =>
        {
            var filePath = Path.Combine(builder.Environment.WebRootPath, "db.json");
            var exists = File.Exists(filePath);
            if (!exists)
                throw new InvalidOperationException("Json file with data not found");

            return new ItemStore(filePath);
        });
    }
}
