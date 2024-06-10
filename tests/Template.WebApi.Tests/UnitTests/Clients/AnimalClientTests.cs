using System.Net;
using System.Text.Json;

using Template.WebApi.Clients;

namespace Template.WebApi.Tests.UnitTests.Clients;

public class AnimalClientTests
{
    private AnimalClient? _sut;

    [Fact]
    public async Task GetAnimals_ReturnsExpectedAnimals()
    {
        //Arrange
        Animal[] expectedAnimals = [new Animal(1, "Dog"), new Animal(2, "Cat")];
        var client = CreateStubbedHttpClient(HttpStatusCode.OK, expectedAnimals);
        _sut = new AnimalClient(client);

        // Act
        var result = await _sut.GetAnimals();

        // Assert
        Assert.Equal(expectedAnimals, result);
    }

    [Fact]
    public async Task GetAnimals_ReturnsEmptyArray_WhenNoAnimalsExistAndStatusIsNotFound()
    {
        //Arrange
        Animal[] expectedAnimals = [];
        var client = CreateStubbedHttpClient(HttpStatusCode.NotFound, expectedAnimals);
        _sut = new AnimalClient(client);

        // Act
        var result = await _sut.GetAnimals();

        // Assert
        Assert.Equal(expectedAnimals, result);
    }

    [Fact]
    public async Task GetAnimals_ThrowsHttpRequestException_WhenStatusCodeIsNotSuccessfull()
    {
        //Arrange
        var client = CreateStubbedHttpClient(HttpStatusCode.InternalServerError, []);
        _sut = new AnimalClient(client);
        async Task Case() => await _sut.GetAnimals();

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(Case);
    }

    [Fact]
    public async Task GetAnimals_ThrowsException_WhenHttpClientThrowsException()
    {
        //Arrange
        var client = CreateStubbedHttpClient(_ => throw new Exception());
        _sut = new AnimalClient(client);
        async Task Case() => await _sut.GetAnimals();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(Case);
    }

    private static HttpClient CreateStubbedHttpClient(HttpStatusCode statusCode, Animal[] expectedAnimals) =>
        new(
            new StubbedHttpMessageHandler(
                new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(JsonSerializer.Serialize(expectedAnimals))
                }))
        { BaseAddress = new Uri("http://localhost") };

    private static HttpClient CreateStubbedHttpClient(Func<HttpRequestMessage, Task<HttpResponseMessage>> responseDelegate) =>
        new(
            new StubbedHttpMessageHandler(responseDelegate))
        { BaseAddress = new Uri("http://localhost") };
}