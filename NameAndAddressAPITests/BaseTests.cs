using NUnit.Framework;

namespace NameAndAddressAPITests;

public abstract class BaseTests
{
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        Setup();
        Mock();
        Run();
    }

    [OneTimeTearDownAttribute]
    public void TearDown()
    {

    }

    public abstract void Run();
    public abstract void Mock();
    public abstract void Setup();
}
