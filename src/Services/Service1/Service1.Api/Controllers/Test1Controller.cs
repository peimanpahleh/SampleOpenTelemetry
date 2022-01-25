
using EventBus.IntegrationEvents.Service1;

namespace Service1.Api.Controllers;

/// <summary>
/// This service is for testing related to rabbitmq and pub/sub
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class Test1Controller : ControllerBase
{
    private readonly ILogger<Test1Controller> _logger;
    private readonly IEventBus _eventBus;

    public Test1Controller(ILogger<Test1Controller> logger, IEventBus eventBus)
    {
        _logger = logger;
        _eventBus = eventBus;
    }

    /// <summary>
    /// endpoint: api/Test1
    /// Publish TestMsg via rabbitMq using MassTransit
    /// This msg will receive by Service2.Api.  filePath =  Service2.Api/EventHandlers/TestMsgHandler
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var msg = new TestMsg()
        {
            Msg = "hello"
        };

        await _eventBus.PublishAsync<TestMsg>(msg);

        _logger.LogInformation("test logger");
        return Ok();
    }
}
