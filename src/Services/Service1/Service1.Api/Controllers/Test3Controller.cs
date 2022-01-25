
namespace Service1.Api.Controllers;

/// <summary>
/// This service is for testing related to grpc
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class Test3Controller : ControllerBase
{
    private readonly GreeterClient _greeterClient;

    public Test3Controller(GreeterClient greeterClient)
    {
        _greeterClient = greeterClient;
    }

    /// <summary>
    /// call grpc service from Service2.Api
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get1()
    {
        var random = new Random().Next(1, 1000);

        var req = new Service2.Api.HelloRequest(){ Name = $"test {random}" };
        var res = await _greeterClient.SayHelloAsync(req);
        return Ok(res);
    }
}
