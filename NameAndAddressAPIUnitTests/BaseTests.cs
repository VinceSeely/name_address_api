using NUnit.Framework;

namespace NameAndAddressAPIUnitTests;

public abstract class BaseTests
{
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        Setup();
        Mock();
        Run();
    }

    [OneTimeTearDown]
    public void TearDown()
    {

    }

    public abstract void Run();
    public abstract void Mock();
    public abstract void Setup();
}
