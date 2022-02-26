using Grpc.Net.Client;
using static Products.Api.MyProductService;

namespace Products.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class BasketsController : ControllerBase
{

    private readonly ILogger<BasketsController> _logger;

    public BasketsController(ILogger<BasketsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// This service is for testing related to grpc, call grpc service from Products.Api
    /// </summary>
    /// <returns></returns>
    [HttpGet("1")]
    public async Task<IActionResult> Get1()
    {
        var handler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var url = "https://productsapi:5011";
        using var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions
        {
            HttpHandler = handler
        });

        var client = new MyProductServiceClient(channel);

        var req = new ProductItemRequest();
        req.Ids = Guid.NewGuid().ToString();

        try
        {
            var res = await client.GetProductsAsync(req);
            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in get products via grpc ErrorMsg:{ex.Message}");
            return BadRequest();
        }
    }

}

