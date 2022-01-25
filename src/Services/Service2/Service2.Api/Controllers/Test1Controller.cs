namespace Service2.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Test1Controller : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
