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

    public override void Mock()
    {
        ravenQueryableMock = new FakeRavenQueryable<Address>(queryableAddresses.AsQueryable());
        documentStoreHolderMock.Setup(x => x.Store).Returns(documentStoreMock.Object);
        documentStoreMock.Setup(x => x.OpenSession()).Returns(documentSessionMock.Object);
        documentSessionMock.Setup(x => x.Query<Address>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(ravenQueryableMock);
    }

    public override void Setup()
    {
        _sut = new AddressManager(loggingMock.Object, documentStoreHolderMock.Object);
    }
}

public class when_checking_if_a_address_exists_and_it_does : AddressManagerTests
{
    private string name;
    private bool result;

    public override void Run()
    {
        result = _sut.AddressExists(name);
    }
    public override void Setup()
    {
        base.Setup();
        name = "one of a kind";
        queryableAddresses.Add(new Address() { Name = name });
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