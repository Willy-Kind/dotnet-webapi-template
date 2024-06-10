using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using Template.WebApi.Clients;

namespace Template.WebApi.Tests.IntegrationTests;

public class GetAnimalTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory;

    private WebApplicationFactory<Program> Fixture(HttpResponseMessage response) =>
        _factory
            .WithWebHostBuilder(builder =>
                builder
                    .ConfigureTestServices(services =>
                        services
                            .AddHttpClient<IAnimalClient, AnimalClient>(client =>
                                client.BaseAddress = new Uri("http://localhost"))
                            .ConfigurePrimaryHttpMessageHandler(() =>
                                new StubbedHttpMessageHandler(_ => Task.FromResult(response)))));

    [Fact]
    public async Task GetAnimals_ReturnsSuccessStatusCode_WhenAnimalsExists()
    {
        // Arrange
        Animal[] animals = [
            new (1, "Panda"),
            new (2, "Rat"),
            new (3, "Goat")
        ];
        HttpResponseMessage response = new(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(animals))
        };
        var client = Fixture(response).CreateClient();

        // Act
        var result = await client.GetAsync("/api/v1/animals");

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equivalent(animals, await result.Content.ReadFromJsonAsync<Animal[]>());
    }

    [Fact]
    public async Task GetAnimals_ReturnsNotFound_WhenAnimalsDoesNotExist()
    {
        //Arrange
        HttpResponseMessage response = new(HttpStatusCode.NotFound)
        {
            Content = new StringContent("[]")
        };
        var client = Fixture(response).CreateClient();

        // Act
        var result = await client.GetAsync("/api/v1/animals");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        Assert.Empty(await result.Content.ReadAsStringAsync());
    }
}
