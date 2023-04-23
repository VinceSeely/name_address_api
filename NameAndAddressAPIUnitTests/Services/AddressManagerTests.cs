using NameAndAddressAPI.Services;
using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using NameAndAddressAPI.Services.DataManagment;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using NameAndAddressAPI.Models;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;
using System.Linq;
using NameAndAddressAPIUnitTests.TestingAids;

namespace NameAndAddressAPIUnitTests.Services;

public abstract class AddressManagerTests : BaseTests
{
    protected AddressManager _sut;
    protected Mock<ILogger<AddressManager>> loggingMock = new Mock<ILogger<AddressManager>>();
    protected Mock<IDocumentStoreHolder> documentStoreHolderMock = new Mock<IDocumentStoreHolder>();
    protected Mock<IDocumentStore> documentStoreMock = new Mock<IDocumentStore>();
    protected List<Address> queryableAddresses = new List<Address>();
    protected Mock<IDocumentSession> documentSessionMock = new Mock<IDocumentSession>();
    protected FakeRavenQueryable<Address> ravenQueryableMock;
    protected RavenConfig config = new()

    {
        DatabaseName = "addresses"
    };

    public override void Mock()
    {
        Address _address = null;
        documentStoreHolderMock.Setup(x => x.Store).Returns(documentStoreMock.Object);
        documentStoreMock.Setup(x => x.OpenSession()).Returns(documentSessionMock.Object);
        documentSessionMock.Setup(x => x.Load<Address>(It.IsAny<string>())).Returns(_address);
        documentSessionMock.Setup(x => x.Query<Address>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(ravenQueryableMock);
    }

    public override void Setup()
    {
        ravenQueryableMock = new FakeRavenQueryable<Address>(queryableAddresses.AsQueryable());
        _sut = new AddressManager(loggingMock.Object, documentStoreHolderMock.Object, config);
    }
}

public class when_checking_if_a_address_exists_and_it_does : AddressManagerTests
{
    private string name;
    private bool result;
    private Address _address;
    public override void Run()
    {
        result = _sut.AddressExists(name);
    }

    public override void Mock()
    {
        base.Mock();
        documentSessionMock.Setup(x => x.Load<Address>($"{config.DatabaseName}/{_address.Name}")).Returns(_address);
    }
    public override void Setup()
    {
        base.Setup();
        name = "one of a kind";
        _address = new()
        {
            Name = name,
            Street = "",
            City = "",
            State = "",
            ZipCode = "",
            Country = "",
        };
    }

    [Test]
    public void then_the_result_should_be_true()
    {
        Assert.IsTrue(result);
    }
}

public class when_checking_if_a_address_exists_and_it_does_not : AddressManagerTests
{
    private string name;
    private bool result;

    public override void Run()
    {
        result = _sut.AddressExists(name);
    }

    [Test]
    public void then_the_result_should_be_false()
    {
        Assert.IsFalse(result);
    }
}

public class when_inserting_an_address_into_the_document_store : AddressManagerTests
{
    private Address _address;
    public override void Run()
    {
        _sut.AddAddress(_address);
    }

    public override void Setup()
    {
        base.Setup();

        _address = new()
        {
            Name = "Specified Name",
            Street = "",
            City = "",
            State = "",
            ZipCode = "",
            Country = "",
        };

    }

    [Test]
    public void then_the_address_should_be_passed_to_the_document_store()
    {
        documentSessionMock.Verify(x => x.Store(_address, $"{config.DatabaseName}/{_address.Name}"));
    }


    [Test]
    public void then_the_changes_should_be_saved()
    {
        documentSessionMock.Verify(x => x.SaveChanges());
    }

}

public class when_updating_an_address_into_the_document_store : AddressManagerTests
{
    private Address _address;
    private Address _originalAddress;
    public override void Run()
    {
        _sut.UpdateAddress(_address);
    }

    public override void Mock()
    {
        base.Mock();
        documentSessionMock.Setup(x => x.Load<Address>($"{config.DatabaseName}/{_address.Name}")).Returns(_originalAddress);
    }

    public override void Setup()
    {
        base.Setup();
        _originalAddress = new()
        {
            Name = "Specified Name",
            Street = "",
            City = "",
            State = "",
            ZipCode = "",
            Country = "",
        };

        queryableAddresses.Add(_originalAddress);
        _address = new()
        {
            Name = "Specified Name",
            Street = "new street",
            City = "new city",
            State = "new state",
            ZipCode = "new zip",
            Country = "new country",
        };
    }

    [Test]
    public void then_the_changes_should_be_saved()
    {
        documentSessionMock.Verify(x => x.SaveChanges());
    }

    [Test]
    public void then_the_document_should_be_loaded_by_the_document_id()
    {
        documentSessionMock.Verify(x => x.Load<Address>($"{config.DatabaseName}/{_address.Name}"));
    }

    [Test]
    public void then_the_original_address_should_have_a_new_street()
    {
        Assert.AreEqual(_originalAddress.Street, _address.Street);
    }

    [Test]
    public void then_the_original_address_should_have_a_new_city()
    {
        Assert.AreEqual(_originalAddress.City, _address.City);
    }

    [Test]
    public void then_the_original_address_should_have_a_new_state()
    {
        Assert.AreEqual(_originalAddress.State, _address.State);
    }

    [Test]
    public void then_the_original_address_should_have_a_new_zip_code()
    {
        Assert.AreEqual(_originalAddress.ZipCode, _address.ZipCode);
    }

    [Test]
    public void then_the_original_address_should_have_a_new_country()
    {
        Assert.AreEqual(_originalAddress.Country, _address.Country);
    }
}

public class when_deleteing_an_address : AddressManagerTests
{
    private string name;
    private Address _address;
    public override void Run()
    {
        _sut.DeleteAddress(name);
    }

    public override void Mock()
    {
        base.Mock();
        //documentSessionMock.Setup(x => x.Load<Address>($"{config.DatabaseName}/{name}"));
    }
    public override void Setup()
    {
        base.Setup();
        name = "one of a kind";
    }

    [Test]
    public void then_the_result_should_be_true()
    {
        documentSessionMock.Verify(x => x.Delete($"{config.DatabaseName}/{name}"));
    }

    [Test]
    public void then_the_changes_should_be_saved()
    {
        documentSessionMock.Verify(x => x.SaveChanges());
    }
}

public class when_getting_all_addresses : AddressManagerTests
{
    private IEnumerable<Address> results;

    public override void Run()
    {
        results = _sut.GetCachedAddresses();
    }

    [Test]
    public void then_the_results_should_be_returned()
    {
        Assert.AreEqual(results, queryableAddresses);
    }

    [Test]
    public void then_query_has_been_called()
    {
        documentSessionMock.Verify(x => x.Query<Address>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
    }
}
