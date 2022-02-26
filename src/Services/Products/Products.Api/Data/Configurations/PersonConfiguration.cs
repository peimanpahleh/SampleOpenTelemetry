using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Products.Api.Data.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);
    }
}
