namespace Products.Api.Controllers;

/// <summary>
/// This service is for testing related to rabbitmq and pub/sub
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProdcutsController : ControllerBase
{

    private readonly ILogger<ProdcutsController> _logger;
    private readonly IEventBus _eventBus;
    private readonly AppDbContext _context;

    public ProdcutsController(ILogger<ProdcutsController> logger, IEventBus eventBus, AppDbContext context)
    {
        _logger = logger;
        _eventBus = eventBus;
        _context = context;
    }

    /// <summary>
    /// endpoint: api/Test1
    /// Publish TestMsg via rabbitMq using MassTransit
    /// This msg will receive by Service2.Api.  filePath =  Service2.Api/EventHandlers/TestMsgHandler
    /// </summary>
    /// <returns></returns>
    [HttpGet("1")]
    public async Task<IActionResult> Get1()
    {
        var prodcutId = Guid.NewGuid().ToString();
        var oldPrice = 8;
        var newPrice = 10;
        var msg = new ProductPriceChanged(prodcutId, oldPrice, newPrice);

        await _eventBus.PublishAsync(msg);

        _logger.LogInformation("ProductPriceChanged published");
        return Ok();
    }

    /// <summary>
    /// endpoint: api/Test2
    /// This service is for testing related to database/Postgres, insert and query to database
    /// </summary>
    /// <returns></returns>
    [HttpGet("2")]
    public async Task<IActionResult> Get2()
    {
        var random = new Random();

        var name = $"p{random.Next(1, 100)}";
        var price = random.Next(10, 40);
        var quantity = random.Next(1, 20);

        var prodcut = new Product(name,price,quantity);

        await _context.Prodcut.AddAsync(prodcut);
        await _context.SaveChangesAsync();


        // note on zipkin tracing when you choose between this two method

        var findPerson = await _context.Prodcut.FindAsync(prodcut.Id);
        //var findPerson = await _context.People.FirstOrDefaultAsync(x => x.Id == person.Id);

        return Ok(findPerson);
    }


}

