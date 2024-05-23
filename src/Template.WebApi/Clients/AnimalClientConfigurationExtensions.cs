using System.Net;
using System.Text.Json;

using Template.WebApi.Clients;

namespace Template.WebApi;

public static class AnimalClientConfigurationExtensions
{
    public static IServiceCollection AddAnimalTypeClient(this IServiceCollection services, IConfiguration configuration)
    {
        var builder = services
            .AddScoped<IExternalAnimalApi, ExternalAnimalApi>()
            .AddHttpClient<IAnimalClient, AnimalClient>(client =>
                client.BaseAddress = new Uri(configuration["AnimalApi:BaseAddress"]!));

        if (configuration.GetSection("AnimalApi:Disabled").Get<bool>())
            builder
                .AddHttpMessageHandler(provider =>
                    new StubbedHttpMessageHandler(
                        provider
                            .GetRequiredService<IExternalAnimalApi>()));

        return services;
    }
}

public class ExternalAnimalApi : IExternalAnimalApi
{
    public HttpResponseMessage GetValueAnimals() =>
        new(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(GenerateRandomAnimals()))
        };

    private static readonly string[] AnimalsTypes = ["Panda", "Rat", "Goat", "Tiger", "Pike", "Zpid3r", "Panther", "Pihranha"];

    private static Animal[] GenerateRandomAnimals() =>
        AnimalsTypes
            .Select(type =>
                new Animal(
                    Random.Shared.Next(0, 55),
                    type))
            .ToArray() ?? [];
}

public interface IExternalAnimalApi
{
    HttpResponseMessage GetValueAnimals();
}
