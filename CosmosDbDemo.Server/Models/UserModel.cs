namespace CosmosDbDemo.Server.Models
{
  public class UserModel : EntityModel
  {
    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
  }
}
