
namespace Service1.Api.Controllers;

/// <summary>
/// This service is for testing related to database/Postgres
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class Test2Controller : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<Test2Controller> _logger;

    public Test2Controller(AppDbContext context, ILogger<Test2Controller> logger)
    {
        _context = context;
        _logger = logger;
    }


    /// <summary>
    /// endpoint: api/Test2
    /// insert and query to database
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var random = new Random().Next(1,1000);
        var person = new Person($"test {random}");

        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();


        // note on zipkin tracing when you choose between this two method

        var findPerson = await _context.People.FindAsync(person.Id);
        //var findPerson = await _context.People.FirstOrDefaultAsync(x => x.Id == person.Id);

        return Ok(findPerson);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get1([FromQuery] string id)
    {

        var findPerson = await _context.People.FindAsync(Guid.Parse(id));

        return Ok(findPerson);
    }
}
