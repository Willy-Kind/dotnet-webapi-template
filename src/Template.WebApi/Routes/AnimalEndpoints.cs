using Asp.Versioning.Builder;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Template.WebApi;

using Template.WebApi.Configuration.Authentication;

namespace Agria.WebApi.Template.Api.Routes;

public static class AnimalEndpoints
{
    public static IEndpointRouteBuilder AddAnimalEndpoints(this IEndpointRouteBuilder endpointRouteBuilder, IConfiguration configuration)
    {
        var builder = endpointRouteBuilder.NewVersionedApi("Animals");

        builder.AddVersion(configuration, 1.0);
        return endpointRouteBuilder;
    }

    private static void AddVersion(this IVersionedEndpointRouteBuilder builder, IConfiguration configuration, double version)
    {
        var group = builder
            .MapGroup("/api/v{version:apiVersion}/animals")
            .HasApiVersion(version);

        group
            .MapGet(string.Empty, GetAnimalsAsync)
            .WithName("GetAnimals")
            .WithOpenApi()
            .ConfigureAuthorizatrion(configuration);
    }

    public static async Task<Results<Ok<Animal[]>, NotFound>> GetAnimalsAsync([FromServices] IAnimalTypeClient animalTypeClient)
    {
        var animals = await animalTypeClient.GetAnimalTypes();
        return animals.Length > 0
            ? TypedResults.Ok(animals)
            : TypedResults.NotFound();
    }
}