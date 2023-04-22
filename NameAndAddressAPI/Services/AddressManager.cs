using NameAndAddressAPI.Models;
using NameAndAddressAPI.Services.DataManagment;

namespace NameAndAddressAPI.Services;

public class AddressManager : IAddressManager
{
    private ILogger<AddressManager> _logger;
    private readonly IDocumentStoreHolder _documentStoreHolder;

    public AddressManager(ILogger<AddressManager> logger, IDocumentStoreHolder documentStoreHolder)
    {
        _logger = logger;
        _documentStoreHolder = documentStoreHolder;
    }

    public void AddAddress(string name, Address address)
    {
        throw new NotImplementedException();
    }

    public bool AddressExists(string name)
    {
        using (var session = _documentStoreHolder.Store.OpenSession())
        {
            var address = session.Query<Address>().SingleOrDefault(x => x.Name.Equals(name));
            return address != null;
        }
    }

    public void UpdateAddress(string name, Address address)
    {
        throw new NotImplementedException();
    }
}
