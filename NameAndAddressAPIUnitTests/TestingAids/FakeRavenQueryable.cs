using Raven.Client.Documents.Session;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents.Queries.Highlighting;
using System.Linq.Expressions;
using System;

namespace NameAndAddressAPIUnitTests.TestingAids;

public class FakeRavenQueryable<T> : IRavenQueryable<T>
{
    private IQueryable<T> source;

    public QueryStatistics QueryStatistics { get; set; }

    public FakeRavenQueryable(IQueryable<T> source, QueryStatistics stats = null)
    {
        this.source = source;
        QueryStatistics = stats;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return source.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return source.GetEnumerator();
    }

    public IRavenQueryable<T> Statistics(out QueryStatistics stats)
    {
        stats = QueryStatistics;
        return this;
    }

    public IRavenQueryable<T> Customize(System.Action<IDocumentQueryCustomization> action)
    {
        return this;
    }

    public IRavenQueryable<T> Highlight(string fieldName, int fragmentLength, int fragmentCount, out Highlightings highlightings)
    {
        throw new System.NotImplementedException();
    }

    public IRavenQueryable<T> Highlight(string fieldName, int fragmentLength, int fragmentCount, HighlightingOptions options, out Highlightings highlightings)
    {
        throw new System.NotImplementedException();
    }

    public IRavenQueryable<T> Highlight(Expression<System.Func<T, object>> path, int fragmentLength, int fragmentCount, out Highlightings highlightings)
    {
        throw new System.NotImplementedException();
    }

    public IRavenQueryable<T> Highlight(Expression<System.Func<T, object>> path, int fragmentLength, int fragmentCount, HighlightingOptions options, out Highlightings highlightings)
    {
        throw new System.NotImplementedException();
    }

    public Type ElementType
    {
        get
        {
            return typeof(T);
        }
    }

    public System.Linq.Expressions.Expression Expression
    {
        get
        {
            return source.Expression;
        }
    }

    public IQueryProvider Provider
    {
        get
        {
            return new FakeRavenQueryProvider(source, QueryStatistics);
        }
    }

    System.Type IQueryable.ElementType => throw new System.NotImplementedException();
}
