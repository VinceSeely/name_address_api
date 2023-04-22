using NameAndAddressAPI.Models;
using Raven.Client.Documents;

namespace NameAndAddressAPI.Services.DataManagment;

public class DocumentStoreHolder : IDocumentStoreHolder
{

    public DocumentStoreHolder(RavenConfig config)
    {
        Store = new DocumentStore
        {
            Urls = new[] { config.Connection },
            Database = config.DatabaseName
        };
        //TODO: check if database exists
        Store.Initialize();
    }

    public IDocumentStore Store { get; }
}
