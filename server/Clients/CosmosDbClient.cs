using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CosmosDbDemo.Server.Options;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CosmosDbDemo.Server.Clients
{
  public class DocumentDbClient : IDatabaseClient
  {
    private readonly CosmosDbOptions _options;
    [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", Justification = "Handled by DI")]
    private readonly DocumentClient _documentClient;
    private readonly ILogger<DocumentDbClient> _logger;

    public DocumentDbClient(
      IOptions<CosmosDbOptions> options,
      ILogger<DocumentDbClient> logger,
      DocumentClient documentClient)
    {
      _options = options.Value;
      _logger = logger;
      _documentClient = documentClient;
    }

    public void Initialize()
    {
    }

        public async Task CreateDatabaseIfDoesntExist(string databaseName)
    {
      _logger.LogInformation($"CreateCollectionIfDoesntExist: databaseName: ${databaseName}");
      try
      {
        await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName });
      }
      catch (Exception e)
      {
        _logger.LogCritical(e, $"CreateDatabaseIfDoesntExist: databaseName: ${databaseName}, message:; {e.Message}");
        throw;
      }
    }

    public async Task CreateCollectionIfDoesntExist(string databaseName, string collectionName)
    {
      _logger.LogInformation($"CreateCollectionIfDoesntExist: databaseName: ${databaseName}, collectionName: {collectionName}");
      try
      {
        await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseName), new DocumentCollection { Id = collectionName });
      }
      catch (Exception e)
      {
        _logger.LogCritical(e, $"CreateCollectionIfDoesntExist: databaseName: ${databaseName}, collectionName: {collectionName},  message:; {e.Message}");
        throw;
      }
    }

    public async Task<T> CreateDocument<T>(string databaseName, string collectionName, T document)
    {
      _logger.LogInformation($"CreateDocument: databaseName: ${databaseName}, collectionName: {collectionName}");
      var newDocument = await _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), document);
      var createdDocument = (T)(dynamic)newDocument.Resource;
      return createdDocument;
    }

    public async Task<T> UpdateDocument<T>(string databaseName, string collectionName, T document)
      where T : EntityModel
    {
      _logger.LogInformation($"CreateDocument: databaseName: ${databaseName}, collectionName: {collectionName}");
      var result = await _documentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, document.Id), document);
      return (T)(dynamic)result.Resource;
    }

    public async Task<IEnumerable<T>> GetDocumentsByExpression<T>(string databaseName, string collectionName, Expression<Func<T, bool>> expression, int maxItemCount = -1)
    {
      List<T> result = new List<T>();

      FeedOptions queryOptions = new FeedOptions { MaxItemCount = maxItemCount };
      IDocumentQuery<T> query = _documentClient.CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
                                  .Where(expression)
                                  .AsDocumentQuery();

      while (query.HasMoreResults)
      {
        foreach (T t in await query.ExecuteNextAsync<T>())
        {
          result.Add(t);
        }
      }

      return result;
    }

    public async Task<T> GetDocumentByExpression<T>(string databaseName, string collectionName, Expression<Func<T, bool>> expression)
    {
      _logger.LogInformation($"GetDocument: databaseName: ${databaseName}, collectionName: {collectionName}");

      FeedOptions queryOptions = new FeedOptions { MaxItemCount = 1 };
      IDocumentQuery<T> query = _documentClient.CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
                                  .Where(expression)
                                  .AsDocumentQuery();

      T result = default(T);
      while (query.HasMoreResults)
      {
        var response = await query.ExecuteNextAsync<T>();
        if (response.Any())
        {
          result = response.Single();
          break;
        }
      }

      return result;
    }
  }
}
