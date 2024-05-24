using System.Net;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

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
                            .AddHttpMessageHandler(() => new StubbedHttpMessageHandler(response))));

    [Fact]
    public async Task GetAnimals_ReturnsSuccessStatusCode()
    {
        // Arrange
        var client = Fixture(new HttpResponseMessage(HttpStatusCode.OK)).CreateClient();

        // Act
        var response = await client.GetAsync("/api/animals");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
