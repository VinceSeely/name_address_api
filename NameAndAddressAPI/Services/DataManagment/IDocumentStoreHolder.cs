﻿using Raven.Client.Documents;

namespace NameAndAddressAPI.Services.DataManagment;

public interface IDocumentStoreHolder
{
    public DocumentStore Store { get; }
}
