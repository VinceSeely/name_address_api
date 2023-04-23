using NameAndAddressAPI.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace NameAndAddressAPI.Services.DataManagment;

public class DocumentStoreHolder : IDocumentStoreHolder
{
    private readonly RavenConfig _config;
    private readonly ILogger<DocumentStoreHolder> _logger;

    public DocumentStoreHolder(RavenConfig config, ILogger<DocumentStoreHolder> logger)
    {
        _config = config;
        _logger = logger;
        Store = new DocumentStore
        {
            Urls = new[] { config.Connection },
            Database = config.DatabaseName
        };
        Store.Initialize();
        
        //TODO: check if database exists
        initializeDatabaseIfNotExist();
    }

    private void initializeDatabaseIfNotExist()
    {
        try
        {
            Store.Maintenance.ForDatabase(_config.DatabaseName).Send(new GetStatisticsOperation());
            _logger.LogInformation($"Database already existsd. Database Name: {_config.DatabaseName}");
        }
        catch (DatabaseDoesNotExistException)
        {
           try
            {
                var newDatabase = new DatabaseRecord(_config.DatabaseName);
                Store.Maintenance.Server.Send(new CreateDatabaseOperation(newDatabase));
                _logger.LogInformation($"Database did not exist so we created a new database. Database Name: {_config.DatabaseName}");
            }
            catch (ConcurrencyException)
            {
                // The database was already created before calling CreateDatabaseOperation
            }

        }
    }

    public IDocumentStore Store { get; }
}
