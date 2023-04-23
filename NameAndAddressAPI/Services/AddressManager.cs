using NameAndAddressAPI.Models;
using NameAndAddressAPI.Services.DataManagment;

namespace NameAndAddressAPI.Services;

public class AddressManager : IAddressManager
{
    private ILogger<AddressManager> _logger;
    private readonly IDocumentStoreHolder _documentStoreHolder;
    private readonly RavenConfig _config;

    public AddressManager(ILogger<AddressManager> logger, IDocumentStoreHolder documentStoreHolder, RavenConfig config)
    {
        _logger = logger;
        _documentStoreHolder = documentStoreHolder;
        _config = config;
    }

    public void AddAddress(Address address)
    {
        using (var session = _documentStoreHolder.Store.OpenSession())
        {
            session.Store(address, $"{_config.DatabaseName}/{address.Name}");
            session.SaveChanges();
        }
    }

    public bool AddressExists(string name)
    {
        using (var session = _documentStoreHolder.Store.OpenSession())
        {
            var address = session.Load<Address>($"{_config.DatabaseName}/{name}");
            return address != null;
        }
    }

    public void UpdateAddress(Address address)
    {
        using (var session = _documentStoreHolder.Store.OpenSession())
        {
            var exsistingAddress = session.Load<Address>($"{_config.DatabaseName}/{address.Name}");
            exsistingAddress.Street = address.Street;
            exsistingAddress.City = address.City;
            exsistingAddress.Country = address.Country;
            exsistingAddress.State = address.State;
            exsistingAddress.ZipCode = address.ZipCode;
            session.SaveChanges();
        }
    }

    public void DeleteAddress(string name)
    {
        using (var session = _documentStoreHolder.Store.OpenSession())
        {
            session.Delete($"{_config.DatabaseName}/{name}");
            session.SaveChanges(); 
        }
    }

    public IEnumerable<Address> GetCachedAddresses()
    {
        using (var session = _documentStoreHolder.Store.OpenSession())
        {
            var addresses = session.Query<Address>().ToList();
            return addresses;
        }
    }
}
