﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CosmosDbDemo.Server.Constants;
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

    public async Task Initialize()
    {
      await CreateDatabaseIfDoesntExist(_options.Databases.CosmosDbDemo);
      await CreateCollectionIfDoesntExist(_options.Databases.CosmosDbDemo, DatabaseConstants.DatabaseNames.Users);
    }

    public async Task CreateDatabaseIfDoesntExist(string databaseName)
    {
        await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName });
    }

    public async Task CreateCollectionIfDoesntExist(string databaseName, string collectionName)
    {
        await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseName), new DocumentCollection { Id = collectionName });
    }

    public async Task<T> CreateDocument<T>(string databaseName, string collectionName, T document)
    {
      var newDocument = await _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), document);
      var createdDocument = (T)(dynamic)newDocument.Resource;
      return createdDocument;
    }

    public async Task<T> UpdateDocument<T>(string databaseName, string collectionName, T document)
      where T : EntityModel
    {
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

    public async Task DeleteDocument(string databaseName, string collectionName, string documentId)
    {
      await _documentClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, documentId));
    }
  }
}
