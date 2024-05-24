using System.Net;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using Template.WebApi.Clients;

namespace Template.WebApi.Tests.IntegrationTests;
public class GetAnimalTests
{
    private static WebApplicationFactory<Program> Fixture(HttpResponseMessage response) =>
        new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
                builder
                    .ConfigureServices(services =>
                        services
                            .AddHttpClient<IAnimalClient, AnimalClient>(client =>
                                client.BaseAddress = new Uri("http://localhost"))
                            .ConfigurePrimaryHttpMessageHandler(() =>
                                new StubbedHttpMessageHandler(_ =>
                                    Task.FromResult(response)))));

    [Fact]
    public async Task GetAnimals_ReturnsSuccessStatusCode_WhenAnimalsExists()
    {
        // Arrange
        var client = Fixture(
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[{\"id\":1,\"name\":\"Dog\"}]")
            })
            .CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/animals");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
