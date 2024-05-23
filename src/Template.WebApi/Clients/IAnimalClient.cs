namespace Template.WebApi;

public interface IAnimalClient
{
    Task<Animal[]> GetAnimals();
}
