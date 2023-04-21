using Microsoft.AspNetCore.Mvc;

namespace NameAndAddressAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<AddressController> _logger;

    public AddressController(ILogger<AddressController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetAddresses")]
    public IEnumerable<String> Get()
    {
        return null;
    }
}
