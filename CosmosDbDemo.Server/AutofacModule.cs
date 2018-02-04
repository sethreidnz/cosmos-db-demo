using System;
using Autofac;
using CosmosDbDemo.Server.Clients;
using CosmosDbDemo.Server.Options;
using CosmosDbDemo.Server.Services;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CosmosDbDemo.Server
{
  public class AutofacModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      // register the DocumentClient for DocumentDB API
      builder.Register(c =>
      {
        var cosmosDbClient = c.Resolve<IOptions<CosmosDbOptions>>().Value;
        return new DocumentClient(cosmosDbClient.EndpointUri, cosmosDbClient.PrimaryKey);
      }).SingleInstance();

      // create the DocumentDB client and initialize it
      builder.Register(c =>
      {
        var databaseClient = new DocumentDbClient(
          c.Resolve<IOptions<CosmosDbOptions>>(),
          c.Resolve<ILogger<DocumentDbClient>>(),
          c.Resolve<DocumentClient>());
        databaseClient.Initialize().Wait();
        return databaseClient;
      }).As<IDatabaseClient>();

      // services
      builder.Register(c => new UserService(c.Resolve<IOptions<CosmosDbOptions>>(), c.Resolve<IDatabaseClient>()))
        .As<IUserService>();
    }
  }
}
