using NameAndAddressAPI.Models;

namespace NameAndAddressAPI.Services;

public interface IAddressManager
{
    bool AddressExists(string name);
    void AddAddress(Address address);
    void UpdateAddress(Address address);
    void DeleteAddress(string name);
    IEnumerable<Address> GetCachedAddresses();
}
