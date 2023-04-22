using NameAndAddressAPI.Models;

namespace NameAndAddressAPI.Services;

public interface IAddressManager
{
    bool AddressExists(string name);
    void AddAddress(string name, Address address);
    void UpdateAddress(string name, Address address);
}
