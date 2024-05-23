using NSubstitute;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute.ExceptionExtensions;
using Template.WebApi.Routes;

namespace Template.WebApi.Tests.UnitTests.Routes;

public class AnimalApiTests
{
    [Fact]
    public async Task GetAnimals_ReturnsOkAnimals_WhenAnimalExists()
    {
        // Arrange
        var animalClient = Substitute.For<IAnimalClient>();
        Animal[] expectedAnimals = [new Animal(1, "Dog")];
        animalClient.GetAnimals().Returns(expectedAnimals);

        //Act
        var result = await AnimalApi.GetAnimals(animalClient);

        // Assert
        Assert.True(result.Result is Ok<Animal[]>);
        Assert.Equal(expectedAnimals, (result.Result as Ok<Animal[]>)!.Value);
    }

    [Fact]
    public async Task GetAnimals_ReturnsNotFound_WhenNoAnimalsExist()
    {
        // Arrange
        var animalClient = Substitute.For<IAnimalClient>();
        Animal[] expectedAnimals = [];
        animalClient.GetAnimals().Returns(expectedAnimals);

        // Act
        var result = await AnimalApi.GetAnimals(animalClient);

        // Assert
        Assert.True(result.Result is NotFound);
    }

    [Fact]
    public async Task GetAnimals_ReturnsInternalServerError_WhenAnimalClientThrowsException()
    {
        // Arrange
        var animalClient = Substitute.For<IAnimalClient>();
        animalClient.GetAnimals().ThrowsAsync(new Exception());
        async Task Case() => await AnimalApi.GetAnimals(animalClient);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(Case);
    }
}
