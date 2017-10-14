using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CosmosDbDemo.Server.Options;
using Microsoft.Azure.Documents.Client;
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

    public Task<T> CreateDocument<T>(string databaseName, string collectionName, object document)
    {
      throw new NotImplementedException();
    }

    public Task<T> GetDocumentByExpression<T>(string databaseName, string collectionName, Expression<Func<T, bool>> expression)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetDocumentsByExpression<T>(string databaseName, string collectionName, Expression<Func<T, bool>> expression, int maxItemCount = -1)
    {
      throw new NotImplementedException();
    }

    public Task<T> UpdateDocument<T>(string databaseName, string collectionName, T document)
      where T : EntityModel
    {
      throw new NotImplementedException();
    }
  }
}
