using System.Net;
using System.Text.Json;

using Template.WebApi.Clients;

namespace Template.WebApi;

public interface IAnimalTypeClient
{
    Task<Animal[]> GetAnimalTypes();
}

public class AnimalTypeClient(HttpClient httpClient) : IAnimalTypeClient
{
    public async Task<Animal[]> GetAnimalTypes()
    {
        var response = await httpClient.GetAsync("api/v1.0/animal-types");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Animal[]>() ?? [];
    }
}

public static class AnimalTypeClientConfigurationExtensions
{
    public static IServiceCollection AddAnimalTypeClient(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddHttpClient<IAnimalTypeClient, AnimalTypeClient>(client =>
                client.BaseAddress = new Uri(configuration["AnimalApi:BaseUrl"]!))
            .ConfigurePrimaryHttpMessageHandler((x) =>
                configuration.GetSection("AnimalApi:Disabled").Get<bool>()
                    ? new MockedHttpMessageHandler(
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
