# Address Api

This is an application that allows users to manage and view a list of addresses via api calls. All endpoints can be viewed via swagger when running in an environment with the environment variable `ASPNETCORE_ENVIRONMENT`  set to `development` or ` docker`.

## Running the application

There are two ways to run the application depending on how you want to use [RavenDB](https://ravendb.net/).

### RavenDB in Docker

If running RavenDB via the docker container run the command `docker compose up --build` or `docker compose up` either one of these will start running both the application in a docker container and a RavenDB in a docker container. After starting the application this way you will need to navigate to [http://localhost:8080/](http://localhost:8080/) and then run through the configuration steps to make sure RavenDB is fully configured to run and work. For this I used an unsecured configuration while doing my testing so if there is a need for a cert to be added to the C# code if you configure it differently you will need to set that up. After everything has been configured you do not need to create any databases as the C# code should handle the creation of the database and then be able to manage all of the data for the Address database.

### RavenDB running locally

If you are running the RavenDB locally you can either use the docker container to run your application or you can run the C# locally on your machine using the command `dotnet run` the only thing you will need to do is go into the `appsettings.Development.json` and update the field `RavenDb.Connection` to be the public url for the ravendb instance.  I have not tested running things this way but this should be the steps to run it.

## Cached Data

We are using the built in caching in RavenDB.Client package to do all of the data caching for this program. 

### Things I Noticed

While testing this I noticed that the caching is used for insert and update to quickly access the documents. If a query is being executed like for get all Addresses the data is cached by the query tags not necessarily by what has been added or not added. so the first time executing the get all endpoint the response time is slower than the second time you execute it. This is due to how RavenDB has configured everything to work when querying for data and interacting with the cache and then saving things to the database backend. 

## Running Unit Tests

To run unit tests you can either run them in the Visual Studio test explorer or on the command line via the command `dotnet test` these test are setup to run and test all buisness logic that was needed to make sure everything was properly called and that the information that was expected was called.
