namespace CosmosDbDemo.Server.Options
{
  public class CosmosDbOptions
  {
    public string EndpointUri { get; set; }

    public string PrimaryKey { get; set; }

    public DatabaseNames Databases { get; set; }
  }

  public class DatabaseNames
  {
    public string CosmosDbDemo { get; set; }
  }
}
