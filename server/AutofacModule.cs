using System;
using Autofac;
using CosmosDbDemo.Server.Clients;
using CosmosDbDemo.Server.Options;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CosmosDbDemo.Server
{
  public class AutofacModule : Module
  {
    private readonly IConfigurationRoot _configuration;

    protected override void Load(ContainerBuilder builder)
    {
      // register the DocumentClient for DocumentDB API
      builder.Register(c => {
        var cosmosDbClient = c.Resolve<IOptions<CosmosDbOptions>>().Value;
        return new DocumentClient(new Uri(cosmosDbClient.EndpointUri), cosmosDbClient.PrimaryKey);
      }).SingleInstance();

      // create the DocumentDB client and initialize it
      builder.Register(c =>
      {
        var databaseClient = new DocumentDbClient(
          c.Resolve<IOptions<CosmosDbOptions>>(),
          c.Resolve<ILogger<DocumentDbClient>>(),
          c.Resolve<DocumentClient>());
        databaseClient.Initialize();
        return databaseClient;
      }).As<IDatabaseClient>();
    }
  }
}
