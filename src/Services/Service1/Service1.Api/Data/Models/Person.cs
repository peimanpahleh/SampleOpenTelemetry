namespace Service1.Api.Data.Models;

public record Person(string Name)
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
