using NUnit.Framework;
using System;
using System.Net.Http;

namespace NameAndAddressAPITests;

public abstract class BaseTests
{
    protected const string _baseUrl = "http://localhost:8000";
    protected string _routeUrl = String.Empty;
    protected HttpClient _httpclient;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        Setup();
        Run();
    }

    [OneTimeTearDown]
    public abstract void TearDown();
    public abstract void Run();
    public virtual void Setup()
    {
        _httpclient = new HttpClient();
        _httpclient.BaseAddress = new Uri(_baseUrl);
    }
}
