using EventBus.IntegrationEvents.Service1;

namespace Service2.Api.EventHandlers;

public class TestMsgHandler : IIntegrationEventHandler<TestMsg>
{
    private readonly ILogger<TestMsgHandler> _logger;

    public TestMsgHandler(ILogger<TestMsgHandler> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TestMsg> context)
    {
        _logger.LogInformation($"msg received: {context.Message.Msg}");
        return Task.CompletedTask;
    }
}
