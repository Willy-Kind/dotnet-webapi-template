using Asp.Versioning.Builder;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Agria.WebApi.Template.Api.Routes;

public static class AnimalEndpoints
{
    public static IEndpointRouteBuilder AddAnimalEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
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

        group
            .MapGet(string.Empty, GetAnimals)
            .WithName("GetAnimals")
            .WithOpenApi();
    }

    public static Results<Ok<Animal[]>, NotFound> GetAnimals()
    {
        var animals = Enumerable
            .Range(0, 3)
            .Select(index =>
                new Animal
                (
                    Random.Shared.Next(0, 55),
                    AnimalsTypes[Random.Shared.Next(AnimalsTypes.Length)]
                ))
            .ToArray() ?? [];

        return animals.Length > 0 ? TypedResults.Ok(animals) : TypedResults.NotFound();
    }

    public record Animal(int Age, string AnimalType);

    private static readonly string[] AnimalsTypes = ["Panda", "Rat", "Goat", "Tiger", "Pike", "Zpid3r", "Panther", "Pihranha"];
}