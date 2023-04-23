using NameAndAddressAPI.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace NameAndAddressAPITests.Controllers;

public abstract class AddressControllerTests : BaseTests
{
    protected List<Address> _addressResults;
    protected List<Address> _expectedAddresses;

    public override void Setup()
    {
        _routeUrl = "/Address";
        base.Setup();
    }

}

public class when_adding_addresses_via_the_api_endpoint : AddressControllerTests
{
    private Address address1;
    private Address address2;
    private Address address3;
    private Address address4;

    public override void Run()
    {
        using Task<HttpResponseMessage> temp = _httpclient.GetAsync($"{_routeUrl}/GetAddresses");
        temp.Wait();
        var results = temp.Result.Content.ReadFromJsonAsync<List<Address>>(); ;
        results.Wait();
        _addressResults = results.Result;
    }
    public override void Setup()
    {
        base.Setup();

        address1 = new()
        {
            Name = "first",
            Street = "street 1",
            City = "city 1",
            State = "state 1",
            ZipCode = "zip 1",
            Country = "country 1",
        };
        address2 = new()
        {
            Name = "second",
            Street = "street 2",
            City = "city 2",
            State = "state 2",
            ZipCode = "zip 2",
            Country = "country 2",
        };
        address3 = new()
        {
            Name = "Third",
            Street = "street 3",
            City = "city 3",
            State = "state 3",
            ZipCode = "zip 3",
            Country = "country 3",
        }; 
        address4 = new()
        {
            Name = "fourth",
            Street = "street 4",
            City = "city 4",
            State = "state 4",
            ZipCode = "zip 4",
            Country = "country 4",
        };
        _expectedAddresses = new List<Address> {
            address1,
            address2,
            address3,
            address4
        }; 
        foreach (var address in _expectedAddresses)
        {
            var json = JsonConvert.SerializeObject(address); // or JsonSerializer.Serialize if using System.Text.Json

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
            using Task<HttpResponseMessage> temp = _httpclient.PostAsync($"{_routeUrl}/AddAddress/", stringContent);
            temp.Wait();
        }
        // Delay to make sure all posts have processed
        Task.Delay(100);
    }

    public override void TearDown()
    {
        foreach(var address in _expectedAddresses)
        {
            using Task<HttpResponseMessage> temp = _httpclient.DeleteAsync($"{_routeUrl}/DeleteAddress/{address.Name}");
            temp.Wait();
        }
    }

    [Test]
    public void then_the_first_address_should_be_in_the_results()
    {
        var address1Result = _addressResults.SingleOrDefault(x => x.Name.Equals(address1.Name));
        Assert.AreEqual(address1Result.Street, address1.Street);
        Assert.AreEqual(address1Result.Country, address1.Country);
        Assert.AreEqual(address1Result.State, address1.State);
        Assert.AreEqual(address1Result.ZipCode, address1.ZipCode);
    }

    [Test]
    public void then_the_second_address_should_be_in_the_results()
    {
        var address2Result = _addressResults.SingleOrDefault(x => x.Name.Equals(address2.Name));
        Assert.AreEqual(address2Result.Street, address2.Street);
        Assert.AreEqual(address2Result.Country, address2.Country);
        Assert.AreEqual(address2Result.State, address2.State);
        Assert.AreEqual(address2Result.ZipCode, address2.ZipCode);
    }

    [Test]
    public void then_the_third_address_should_be_in_the_results()
    {
        var address3Result = _addressResults.SingleOrDefault(x => x.Name.Equals(address3.Name));
        Assert.AreEqual(address3Result.Street, address3.Street);
        Assert.AreEqual(address3Result.Country, address3.Country);
        Assert.AreEqual(address3Result.State, address3.State);
        Assert.AreEqual(address3Result.ZipCode, address3.ZipCode);
    }
    
    [Test]
    public void then_the_fourth_address_should_be_in_the_results()
    {
        var address4Result = _addressResults.SingleOrDefault(x => x.Name.Equals(address4.Name));
        Assert.AreEqual(address4Result.Street, address4.Street);
        Assert.AreEqual(address4Result.Country, address4.Country);
        Assert.AreEqual(address4Result.State, address4.State);
        Assert.AreEqual(address4Result.ZipCode, address4.ZipCode);

    }
}

public class when_updating_an_address_via_the_api_endpoint : AddressControllerTests
{
    private Address address1;
    private Address address2;
    private Address address3;
    private Address address4;

    public override void Run()
    {
        var json = JsonConvert.SerializeObject(address3); // or JsonSerializer.Serialize if using System.Text.Json

        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
        using Task<HttpResponseMessage> putTask = _httpclient.PutAsync($"{_routeUrl}/UpdateAddress/", stringContent);
        putTask.Wait();
        using Task<HttpResponseMessage> getTask = _httpclient.GetAsync($"{_routeUrl}/GetAddresses");
        getTask.Wait();
        var results = getTask.Result.Content.ReadFromJsonAsync<List<Address>>(); ;
        results.Wait();
        _addressResults = results.Result;
    }
    public override void Setup()
    {
        base.Setup();

        address1 = new()
        {
            Name = "first",
            Street = "street 1",
            City = "city 1",
            State = "state 1",
            ZipCode = "zip 1",
            Country = "country 1",
        };
        address2 = new()
        {
            Name = "second",
            Street = "street 2",
            City = "city 2",
            State = "state 2",
            ZipCode = "zip 2",
            Country = "country 2",
        };
        address3 = new()
        {
            Name = "Third",
            Street = "street 3",
            City = "city 3",
            State = "state 3",
            ZipCode = "zip 3",
            Country = "country 3",
        };
        address4 = new()
        {
            Name = "fourth",
            Street = "street 4",
            City = "city 4",
            State = "state 4",
            ZipCode = "zip 4",
            Country = "country 4",
        };
        _expectedAddresses = new List<Address> {
            address1,
            address2,
            address3,
            address4
        };
        foreach (var address in _expectedAddresses)
        {
            var json = JsonConvert.SerializeObject(address); // or JsonSerializer.Serialize if using System.Text.Json

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
            using Task<HttpResponseMessage> temp = _httpclient.PostAsync($"{_routeUrl}/AddAddress/", stringContent);
            temp.Wait();
        }
        // Delay to make sure all posts have processed
        Task.Delay(100);
        address3.Street = "updated street";
        address3.City = "not your city";
    }

    public override void TearDown()
    {
        foreach (var address in _expectedAddresses)
        {
            using Task<HttpResponseMessage> temp = _httpclient.DeleteAsync($"{_routeUrl}/DeleteAddress/{address.Name}");
            temp.Wait();
        }
    }

    [Test]
    public void then_the_first_address_should_be_in_the_results()
    {
        var address1Result = _addressResults.SingleOrDefault(x => x.Name.Equals(address1.Name));
        Assert.AreEqual(address1Result.Street, address1.Street);
        Assert.AreEqual(address1Result.Country, address1.Country);
        Assert.AreEqual(address1Result.State, address1.State);
        Assert.AreEqual(address1Result.ZipCode, address1.ZipCode);
    }

    [Test]
    public void then_the_second_address_should_be_in_the_results()
    {
        var address2Result = _addressResults.SingleOrDefault(x => x.Name.Equals(address2.Name));
        Assert.AreEqual(address2Result.Street, address2.Street);
        Assert.AreEqual(address2Result.Country, address2.Country);
        Assert.AreEqual(address2Result.State, address2.State);
        Assert.AreEqual(address2Result.ZipCode, address2.ZipCode);
    }

    [Test]
    public void then_the_third_address_should_be_in_the_results()
    {
        var address3Result = _addressResults.SingleOrDefault(x => x.Name.Equals(address3.Name));
        Assert.AreEqual(address3Result.Street, address3.Street);
        Assert.AreEqual(address3Result.Country, address3.Country);
        Assert.AreEqual(address3Result.State, address3.State);
        Assert.AreEqual(address3Result.ZipCode, address3.ZipCode);
    }

    [Test]
    public void then_the_fourth_address_should_be_in_the_results()
    {
        var address4Result = _addressResults.SingleOrDefault(x => x.Name.Equals(address4.Name));
        Assert.AreEqual(address4Result.Street, address4.Street);
        Assert.AreEqual(address4Result.Country, address4.Country);
        Assert.AreEqual(address4Result.State, address4.State);
        Assert.AreEqual(address4Result.ZipCode, address4.ZipCode);

    }
}

public class when_deleteing_an_address_via_the_api_endpoint : AddressControllerTests
{
    private Address address1;
    private Address address2;
    private Address address3;
    private Address address4;

    public override void Run()
    {
        var json = JsonConvert.SerializeObject(address3); // or JsonSerializer.Serialize if using System.Text.Json


        using Task<HttpResponseMessage> temp = _httpclient.DeleteAsync($"{_routeUrl}/DeleteAddress/{address2.Name}");
        temp.Wait();
        using Task<HttpResponseMessage> getTask = _httpclient.GetAsync($"{_routeUrl}/GetAddresses");
        getTask.Wait();
        var results = getTask.Result.Content.ReadFromJsonAsync<List<Address>>(); ;
        results.Wait();
        _addressResults = results.Result;
    }
    public override void Setup()
    {
        base.Setup();

        address1 = new()
        {
            Name = "first",
            Street = "street 1",
            City = "city 1",
            State = "state 1",
            ZipCode = "zip 1",
            Country = "country 1",
        };
        address2 = new()
        {
            Name = "second",
            Street = "street 2",
            City = "city 2",
            State = "state 2",
            ZipCode = "zip 2",
            Country = "country 2",
        };
        address3 = new()
        {
            Name = "Third",
            Street = "street 3",
            City = "city 3",
            State = "state 3",
            ZipCode = "zip 3",
            Country = "country 3",
        };
        address4 = new()
        {
            Name = "fourth",
            Street = "street 4",
            City = "city 4",
            State = "state 4",
            ZipCode = "zip 4",
            Country = "country 4",
        };
        _expectedAddresses = new List<Address> {
            address1,
            address2,
            address3,
            address4
        };
        foreach (var address in _expectedAddresses)
        {
            var json = JsonConvert.SerializeObject(address); // or JsonSerializer.Serialize if using System.Text.Json

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
            using Task<HttpResponseMessage> temp = _httpclient.PostAsync($"{_routeUrl}/AddAddress/", stringContent);
            temp.Wait();
        }
        // Delay to make sure all posts have processed
        Task.Delay(100);
    }

    public override void TearDown()
    {
        foreach (var address in _expectedAddresses)
        {
            using Task<HttpResponseMessage> temp = _httpclient.DeleteAsync($"{_routeUrl}/DeleteAddress/{address.Name}");
            temp.Wait();
        }
    }

    [Test]
    public void then_the_first_address_should_be_in_the_results()
    {
        var address1Result = _addressResults.SingleOrDefault(x => x.Name.Equals(address1.Name));
        Assert.AreEqual(address1Result.Street, address1.Street);
        Assert.AreEqual(address1Result.Country, address1.Country);
        Assert.AreEqual(address1Result.State, address1.State);
        Assert.AreEqual(address1Result.ZipCode, address1.ZipCode);
    }

    [Test]
    public void then_the_second_address_should_not_be_in_the_results()
    {
        var address2Result = _addressResults.SingleOrDefault(x => x.Name.Equals(address2.Name));
        Assert.IsNull(address2Result);
    }

    [Test]
    public void then_the_third_address_should_be_in_the_results()
    {
        var address3Result = _addressResults.SingleOrDefault(x => x.Name.Equals(address3.Name));
        Assert.AreEqual(address3Result.Street, address3.Street);
        Assert.AreEqual(address3Result.Country, address3.Country);
        Assert.AreEqual(address3Result.State, address3.State);
        Assert.AreEqual(address3Result.ZipCode, address3.ZipCode);
    }

    [Test]
    public void then_the_fourth_address_should_be_in_the_results()
    {
        var address4Result = _addressResults.SingleOrDefault(x => x.Name.Equals(address4.Name));
        Assert.AreEqual(address4Result.Street, address4.Street);
        Assert.AreEqual(address4Result.Country, address4.Country);
        Assert.AreEqual(address4Result.State, address4.State);
        Assert.AreEqual(address4Result.ZipCode, address4.ZipCode);

    }
}

public class when_adding_a_duplicate_address_via_the_api_endpoint : AddressControllerTests
{
    private Address address1;
    private Address address2;
    private Address address3;
    private Address address4;
    private string result;

    public override void Run()
    {

        var json = JsonConvert.SerializeObject(address2); // or JsonSerializer.Serialize if using System.Text.Json

        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
        using Task<HttpResponseMessage> temp = _httpclient.PostAsync($"{_routeUrl}/AddAddress/", stringContent);
        temp.Wait();
        var results = temp.Result.Content.ReadAsStringAsync();
        results.Wait();
        result = results.Result;
    }
    public override void Setup()
    {
        base.Setup();

        address1 = new()
        {
            Name = "first",
            Street = "street 1",
            City = "city 1",
            State = "state 1",
            ZipCode = "zip 1",
            Country = "country 1",
        };
        address2 = new()
        {
            Name = "second",
            Street = "street 2",
            City = "city 2",
            State = "state 2",
            ZipCode = "zip 2",
            Country = "country 2",
        };
        address3 = new()
        {
            Name = "Third",
            Street = "street 3",
            City = "city 3",
            State = "state 3",
            ZipCode = "zip 3",
            Country = "country 3",
        };
        address4 = new()
        {
            Name = "fourth",
            Street = "street 4",
            City = "city 4",
            State = "state 4",
            ZipCode = "zip 4",
            Country = "country 4",
        };
        _expectedAddresses = new List<Address> {
            address1,
            address2,
            address3,
            address4
        };
        foreach (var address in _expectedAddresses)
        {
            var json = JsonConvert.SerializeObject(address); // or JsonSerializer.Serialize if using System.Text.Json

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
            using Task<HttpResponseMessage> temp = _httpclient.PostAsync($"{_routeUrl}/AddAddress/", stringContent);
            temp.Wait();
        }
        // Delay to make sure all posts have processed
        Task.Delay(100);
    }

    public override void TearDown()
    {
        foreach (var address in _expectedAddresses)
        {
            using Task<HttpResponseMessage> temp = _httpclient.DeleteAsync($"{_routeUrl}/DeleteAddress/{address.Name}");
            temp.Wait();
        }
    }

    [Test]
    public void then_the_resutl_should_say_failed_to_add_as_it_already_exists()
    {
        Assert.AreEqual(result, $"failed to insert there is already an entry for {address2.Name}");
    }
}

public class when_updating_a_address_that_does_not_exist_via_the_api_endpoint : AddressControllerTests
{
    private Address address1;
    private Address address2;
    private Address address3;
    private Address address4;
    private Address address5;
    private string result;

    public override void Run()
    {

        var json = JsonConvert.SerializeObject(address5); // or JsonSerializer.Serialize if using System.Text.Json

        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
        using Task<HttpResponseMessage> temp = _httpclient.PutAsync($"{_routeUrl}/UpdateAddress/", stringContent);
        temp.Wait();
        var results = temp.Result.Content.ReadAsStringAsync();
        results.Wait();
        result = results.Result;
    }
    public override void Setup()
    {
        base.Setup();

        address1 = new()
        {
            Name = "first",
            Street = "street 1",
            City = "city 1",
            State = "state 1",
            ZipCode = "zip 1",
            Country = "country 1",
        };
        address2 = new()
        {
            Name = "second",
            Street = "street 2",
            City = "city 2",
            State = "state 2",
            ZipCode = "zip 2",
            Country = "country 2",
        };
        address3 = new()
        {
            Name = "Third",
            Street = "street 3",
            City = "city 3",
            State = "state 3",
            ZipCode = "zip 3",
            Country = "country 3",
        };
        address4 = new()
        {
            Name = "fourth",
            Street = "street 4",
            City = "city 4",
            State = "state 4",
            ZipCode = "zip 4",
            Country = "country 4",
        };
        address5 = new()
        {
            Name = "fith does not exist",
            Street = "street 5",
            City = "city 5",
            State = "state 5",
            ZipCode = "zip 5",
            Country = "country 5",
        };
        _expectedAddresses = new List<Address> {
            address1,
            address2,
            address3,
            address4
        };
        foreach (var address in _expectedAddresses)
        {
            var json = JsonConvert.SerializeObject(address); // or JsonSerializer.Serialize if using System.Text.Json

            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
            using Task<HttpResponseMessage> temp = _httpclient.PostAsync($"{_routeUrl}/AddAddress/", stringContent);
            temp.Wait();
        }
        // Delay to make sure all posts have processed
        Task.Delay(100);
    }

    public override void TearDown()
    {
        foreach (var address in _expectedAddresses)
        {
            using Task<HttpResponseMessage> temp = _httpclient.DeleteAsync($"{_routeUrl}/DeleteAddress/{address.Name}");
            temp.Wait();
        }
    }

    [Test]
    public void then_the_resutl_should_say_failed_to_add_as_it_already_exists()
    {
        Assert.AreEqual(result, $"Failed to update {address5.Name}. No such address exists so it cannot be updated.");
    }
}
