using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Template.WebApi.Configuration.Authentication;

namespace Template.WebApi.Routes;

public static class AnimalApi
{
    public static IEndpointRouteBuilder RegisterAnimalEndpoints(
        this IEndpointRouteBuilder endpointRouteBuilder,
        IConfiguration configuration)
    {
        var group = endpointRouteBuilder
            .NewVersionedApi("Animals")
            .MapGroup("/api/v{version:apiVersion}/animals")
            .HasApiVersion(1.0);

        group
           .MapGet(string.Empty, GetAnimals)
           .WithName("GetAnimals")
           .WithOpenApi()
           .ConfigureAuthorizatrion(configuration);

        return endpointRouteBuilder;
    }

    public static async Task<Results<Ok<Animal[]>, NotFound>> GetAnimals(
        [FromServices] IAnimalClient animalTypeClient)
    {
        var animals = await animalTypeClient.GetAnimals();
        return animals.Length > 0
            ? TypedResults.Ok(animals)
            : TypedResults.NotFound();
    }
}