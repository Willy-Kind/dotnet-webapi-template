using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Agria.WebApi.Template.Api.Routes;

internal static class AnimalEndpoints
{
    public static IEndpointRouteBuilder AddWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder, IConfiguration configuration)
    {
        var builder = endpointRouteBuilder.NewVersionedApi("Animals");

        builder.AddVersion(1.0);
        return endpointRouteBuilder;
    }

    private static void AddVersion(this IVersionedEndpointRouteBuilder builder, double version)
    {
        var group = builder
                   .MapGroup("/api/v{version:apiVersion}/animals")
                   .HasApiVersion(version);

        group.MapGet("/weatherforecast", () =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
            new Animal
            (
                Random.Shared.Next(0, 55),
                animals[Random.Shared.Next(animals.Length)]
            ))
            .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast")
        .WithOpenApi();
    }

    public async static Task<Results<Ok<string[]>, NotFound>> GetAnimails()
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
        new Animal
        (
            Random.Shared.Next(0, 55),
            animals[Random.Shared.Next(animals.Length)]
        ))
        .ToArray();
        await Task.CompletedTask;
        if (string.IsNullOrEmpty("Animails"))
            return TypedResults.NotFound();

        return TypedResults.Ok(new string[] { "" });
    }

    record Animal(int Age, string AnimalType);

    private static readonly string[] animals = ["Panda", "Rat", "Goats", "Tigers", "Pikes", "Zpid3r", "Panthers", "Pihranha"];
}