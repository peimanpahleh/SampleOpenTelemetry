namespace Products.Api.Services;

public class ProductService : MyProductService.MyProductServiceBase
{

    public override async Task<ProductListResponse> GetProducts(ProductItemRequest request, ServerCallContext context)
    {
        var res = new ProductListResponse();
        var random = new Random();

        for (int i = 0; i < 10; i++)
        {
            var price = random.Next(5,20);
            var quantity = random.Next(1,15);
            var product = new ProductItemResponse()
            {
                Id = Guid.NewGuid().ToString(),
                ImageId = Guid.NewGuid().ToString(),
                Name = $"p{i}",
                Price = price,
                PriceId = Guid.NewGuid().ToString(),
                Quantity = quantity,
            };
            res.Product.Add(product);
        }

        return res;
    }
}
