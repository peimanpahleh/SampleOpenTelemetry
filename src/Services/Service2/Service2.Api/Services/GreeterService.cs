
namespace Service2.Api.Services;

public class GreeterService : Greeter.GreeterBase
{
    private static readonly ActivitySource ActivitySource = new(nameof(GreeterService));

    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        using var activity = ActivitySource.StartActivity(nameof(SayHello));

        activity?.SetTag("msg", request.Name);

        // something that takes a bit
        await Task.Delay(TimeSpan.FromMilliseconds(250));

        return new HelloReply { Message = "Hello " + request.Name };
    }
}
