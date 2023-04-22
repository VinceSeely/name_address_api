using Microsoft.AspNetCore.Mvc;
using NameAndAddressAPI.Models;
using NameAndAddressAPI.Services;
using System.Net;

namespace NameAndAddressAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressController : ControllerBase
{
    private readonly ILogger<AddressController> _logger;
    private readonly IAddressManager _addressManager;

    public AddressController(ILogger<AddressController> logger, IAddressManager addressManager)
    {
        _logger = logger;
        _addressManager = addressManager;
    }

    [HttpGet(Name = "GetAddresses")]
    public IEnumerable<String> Get()
    {
        return null;
    }

    [HttpPut(Name = "AddAddress")]
    public ActionResult<string> InsertAddress([FromQuery] string name, [FromBody] Address address)
    {
        if (_addressManager.AddressExists(name))
        {
            _logger.LogInformation($"attempted to insert a duplicate entry for {name}");
            return $"failed to insert there is already an entry for {name}";
        }
        _addressManager.AddAddress(name, address);
        return "Successfully uploaded";
    }

    public ActionResult<string> UpdateAddress(string name, Address address)
    {
        if (!_addressManager.AddressExists(name))
        {
            return $"Failed to update {name}. No such address exists so it cannot be updated.";
        }
        _addressManager.UpdateAddress(name, address);
        return "Successfully updated";
    }
}
