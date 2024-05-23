using System.Net;
using System.Text.Json;

using Template.WebApi.Clients;

namespace Template.WebApi;

public static class AnimalClientConfigurationExtensions
{
    public static IServiceCollection AddAnimalTypeClient(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddHttpClient<AnimalClient, AnimalClient>(client =>
                client.BaseAddress = new Uri(configuration["AnimalApi:BaseUrl"]!))
            .ConfigurePrimaryHttpMessageHandler((x) =>
                configuration.GetSection("AnimalApi:Disabled").Get<bool>()
                    ? new StubbedHttpMessageHandler(
                        new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(JsonSerializer.Serialize(GetValueAnimals))
                        })
                    : new HttpClientHandler())
            .Services;

    private static readonly string[] AnimalsTypes = ["Panda", "Rat", "Goat", "Tiger", "Pike", "Zpid3r", "Panther", "Pihranha"];

    private static readonly Animal[] GetValueAnimals = Enumerable
        .Range(0, 3)
        .Select(index =>
            new Animal
            (
                Random.Shared.Next(0, 55),
                AnimalsTypes[Random.Shared.Next(AnimalsTypes.Length)]
            ))
        .ToArray() ?? [];
}
