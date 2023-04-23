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

    [HttpPost(Name = "AddAddress")]
    public ActionResult<string> InsertAddress([FromBody] Address address)
    {
        if (_addressManager.AddressExists(address.Name))
        {
            _logger.LogInformation($"attempted to insert a duplicate entry for {address.Name}");
            return $"failed to insert there is already an entry for {address.Name}";
        }
        _addressManager.AddAddress(address);
        return "Successfully uploaded";
    }


    [HttpPut(Name = "UpdateAddress")]
    public ActionResult<string> UpdateAddress([FromBody] Address address)
    {
        if (!_addressManager.AddressExists(address.Name))
        {
            return $"Failed to update {address.Name}. No such address exists so it cannot be updated.";
        }
        _addressManager.UpdateAddress(address);
        return "Successfully updated";
    }

    [HttpDelete(Name = "DeleteAddress")]
    public ActionResult<string> DeleteAddress(string name)
    {
        if (!_addressManager.AddressExists(name))
        {
            return $"Failed to delete {name}. No such address exists so it cannot be deleted.";
        }
        _addressManager.DeleteAddress(name);
        return $"Successfully deleted {name}";

    }
}
