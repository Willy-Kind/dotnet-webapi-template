using System.Net;

namespace Template.WebApi;

public class AnimalClient(HttpClient httpClient) : IAnimalClient
{
    public async Task<Animal[]> GetAnimals()
    {
        var response = await httpClient.GetAsync("api/v1.0/animals");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Animal[]>() ?? [];

    }
}
