using NameAndAddressAPI.Controllers;
using Moq;
using Microsoft.Extensions.Logging;
using NameAndAddressAPI.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using NameAndAddressAPI.Services;
using System;
using System.Collections.Generic;

namespace NameAndAddressAPIUnitTests.Controllers;

public abstract class AddressControllerTests : BaseTests
{
    protected AddressController _sut;
    protected Mock<ILogger<AddressController>> loggingMock = new Mock<ILogger<AddressController>>();
    protected Mock<IAddressManager> AddressManagerMock = new Mock<IAddressManager>();

    public override void Mock()
    {
    }

    public override void Setup()
    {
        _sut = new(loggingMock.Object, AddressManagerMock.Object);
    }

}

public class when_adding_an_address_to_the_database : AddressControllerTests
{
    private string name;
    private Address address;
    private ActionResult<string> result;

    public override void Run()
    {
        result = _sut.InsertAddress(address);
    }

    public override void Mock()
    {
        base.Mock();
        AddressManagerMock.Setup(x => x.AddressExists(name)).Returns(false);
    }
    public override void Setup()
    {
        name = "unique name 1";
        address = new()
        {
            Name = name,
            Street = "",
            City = "",
            State = "",
            ZipCode = "",
            Country = "",
        };
        base.Setup();
    }

    [Test]
    public void then_there_should_be_a_successfully_add_message()
    {
        Assert.AreEqual(result.Value, "Successfully uploaded");
    }

    [Test]
    public void then_the_address_manager_is_called_to_add_the_address_and_name_to_the_database()
    {
        AddressManagerMock.Verify(x => x.AddAddress(address));
    }
}

public class when_adding_an_address_to_the_database_and_the_name_already_exists : AddressControllerTests
{
    private string name;
    private Address address;
    private ActionResult<string> result;

    public override void Run()
    {
        result = _sut.InsertAddress(address);
    }

    public override void Mock()
    {
        base.Mock();
        AddressManagerMock.Setup(x => x.AddressExists(name)).Returns(true);
    }

    public override void Setup()
    {
        name = "unique name 1";
        address = new()
        {
            Name = name,
            Street = "",
            City = "",
            State = "",
            ZipCode = "",
            Country = "",
        };
        base.Setup();
    }

    [Test]
    public void then_there_should_be_a_successfully_add_message()
    {
        Assert.AreEqual(result.Value, $"failed to insert there is already an entry for {name}");
    }

    [Test]
    public void then_there_should_be_an_info_log_that_an_attempt_to_add_a_duplicate_entry_has_occured()
    {
        loggingMock.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == $"attempted to insert a duplicate entry for {name}" && @type.Name == "FormattedLogValues"),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()));
    }
}

public class when_updating_an_address_in_the_database : AddressControllerTests
{
    private string name;
    private Address address;
    private ActionResult<string> result;

    public override void Run()
    {
        result = _sut.UpdateAddress(address);
    }

    public override void Mock()
    {
        base.Mock();
        AddressManagerMock.Setup(x => x.AddressExists(name)).Returns(true);
    }
    public override void Setup()
    {
        name = "unique name 1";
        address = new()
        {
            Name = name,
            Street = "",
            City = "",
            State = "",
            ZipCode = "",
            Country = "",
        };
        base.Setup();
    }

    [Test]
    public void then_there_should_be_a_successfully_add_message()
    {
        Assert.AreEqual(result.Value, "Successfully updated");
    }

    [Test]
    public void then_the_address_manager_is_called_to_update_the_address_and_name_to_the_database()
    {
        AddressManagerMock.Verify(x => x.UpdateAddress(address));
    }
}

public class when_updating_an_address_in_the_database_but_the_name_does_not_exsit : AddressControllerTests
{
    private string name;
    private Address address;
    private ActionResult<string> result;

    public override void Run()
    {
        result = _sut.UpdateAddress(address);
    }

    public override void Mock()
    {
        base.Mock();
        AddressManagerMock.Setup(x => x.AddressExists(name)).Returns(false);
    }
    public override void Setup()
    {
        name = "unique name 1";
        address = new()
        {
            Name = name,
            Street = "",
            City = "",
            State = "",
            ZipCode = "",
            Country = "",
        };
        base.Setup();
    }

    [Test]
    public void then_there_should_be_a_successfully_update_message()
    {
        Assert.AreEqual(result.Value, $"Failed to update {name}. No such address exists so it cannot be updated.");
    }
}


public class when_deleteing_a_address_from_the_table_and_it_does_exist : AddressControllerTests
{
    private string name;
    private ActionResult<string> result;

    public override void Run()
    {
        result = _sut.DeleteAddress(name);
    }
    public override void Mock()
    {
        base.Mock();
        AddressManagerMock.Setup(x => x.AddressExists(name)).Returns(true);
    }

    [Test]
    public void then_the_method_to_delete_should_be_called()
    {
        AddressManagerMock.Verify(x => x.DeleteAddress(name));
    }

    [Test]
    public void then_there_should_be_a_successfully_deleted_message()
    {
        Assert.AreEqual(result.Value, $"Successfully deleted {name}");
    }
}
public class when_deleteing_a_address_from_the_table_and_it_does_not_exist : AddressControllerTests
{
    private string name;
    private ActionResult<string> result;

    public override void Run()
    {
        result = _sut.DeleteAddress(name);
    }
    public override void Mock()
    {
        base.Mock();
        AddressManagerMock.Setup(x => x.AddressExists(name)).Returns(false);
    }

    [Test]
    public void then_the_method_to_delete_should_not_be_called()
    {
        AddressManagerMock.Verify(x => x.DeleteAddress(name), Times.Never());
    }


    [Test]
    public void then_there_should_be_a_failed_to_delete_message()
    {
        Assert.AreEqual(result.Value, $"Failed to delete {name}. No such address exists so it cannot be deleted.");
    }
}

public class when_getting_all_addresses : AddressControllerTests
{
    private string name;
    private ActionResult<IEnumerable<Address>> result;
    IEnumerable<Address> expected;

    public override void Run()
    {
        result = _sut.GetAllAddresses();
    }

    public override void Mock()
    {
        base.Mock();
        AddressManagerMock.Setup(x => x.GetCachedAddresses()).Returns(expected);
    }

    public override void Setup()
    {
        base.Setup();
        expected = new List<Address>() { new Address() { Name="entry 1"} };
    }

    [Test]
    public void then_the_method_to_delete_should_not_be_called()
    {
        AddressManagerMock.Verify(x => x.GetCachedAddresses());
    }


    [Test]
    public void then_there_should_be_a_failed_to_delete_message()
    {
        Assert.AreEqual(result.Value, expected);
    }
}