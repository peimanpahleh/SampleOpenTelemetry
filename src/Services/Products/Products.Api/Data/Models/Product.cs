namespace Products.Api.Data.Models;

public record Product(string Name,long Price,int Quantity)
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
