using System.Net;
using System.Text.Json;

using Template.WebApi.Clients;

namespace Template.WebApi;

public static class AnimalClientConfigurationExtensions
{
    public static IServiceCollection AddAnimalTypeClient(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddHttpClient<IAnimalClient, AnimalClient>(client =>
                client.BaseAddress = new Uri(configuration["AnimalApi:BaseAddress"]!))
            .ConfigurePrimaryHttpMessageHandler((x) =>
                configuration.GetSection("AnimalApi:Disabled").Get<bool>()
                    ? new StubbedHttpMessageHandler(
                        new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(JsonSerializer.Serialize(GetValueAnimals()))
                        })
                    : new HttpClientHandler())
            .Services;

    private static readonly string[] AnimalsTypes = ["Panda", "Rat", "Goat", "Tiger", "Pike", "Zpid3r", "Panther", "Pihranha"];
    private static Animal[] GetValueAnimals() =>
        AnimalsTypes
            .Select(type =>
                new Animal(
                    Random.Shared.Next(0, 55),
                    type))
            .ToArray() ?? [];
}
