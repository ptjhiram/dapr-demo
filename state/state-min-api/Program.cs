using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Dapr.Client;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DaprClient>(new DaprClientBuilder().Build());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/state/{key}", async (DaprClient client, string key) =>
{
    var state = await client.GetStateAsync<string>("statestore", key);
    if (state != null)
    {
        return Results.Ok(state);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("/state", async (DaprClient client, StateData stateData) =>
{
    await client.SaveStateAsync("statestore", stateData.key, stateData.data);
    return Results.Created($"/state/{stateData.key}", stateData);
});

app.MapDelete("/state/{key}", async (DaprClient client, string key) =>
{
    await client.DeleteStateAsync("statestore", key);
    return Results.Ok();
});

app.Run();

public record StateData([property: JsonPropertyName("key")] string key, [property: JsonPropertyName("data")] string data);