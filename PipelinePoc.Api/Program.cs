using PipelinePoc.Api;
using PipelinePoc.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(b => b.AddDefaultPolicy(o => o.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

ServiceRegistry.AddAppServices(builder);

builder.Host.UseDefaultServiceProvider((_, options) =>
{
    options.ValidateOnBuild = true;
    options.ValidateScopes = true;
});

var app = builder.Build();

EndpointsMap.Map(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<HttpMiddleware>();
app.UseCors();

app.Run();