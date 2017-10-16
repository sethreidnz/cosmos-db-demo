using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CosmosDbDemo.Server.Clients
{
  public interface IDatabaseClient
  {
    Task CreateDatabaseIfDoesntExist(string databaseName);

    Task CreateCollectionIfDoesntExist(string databaseName, string collectionName);

    Task<T> CreateDocument<T>(string databaseName, string collectionName, T document);

    Task<T> GetDocumentByExpression<T>(string databaseName, string collectionName, Expression<Func<T, bool>> expression);

    Task<IEnumerable<T>> GetDocumentsByExpression<T>(string databaseName, string collectionName, Expression<Func<T, bool>> expression, int maxItemCount = -1);

    Task<T> UpdateDocument<T>(string databaseName, string collectionName, T document)
      where T : EntityModel;

    Task DeleteDocument(string databaseName, string collectionName, string documentId);
  }
}
