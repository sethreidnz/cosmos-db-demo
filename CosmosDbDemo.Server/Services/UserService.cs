using System;
using System.Threading.Tasks;
using CosmosDbDemo.Server.Clients;
using CosmosDbDemo.Server.Constants;
using CosmosDbDemo.Server.Extensions;
using CosmosDbDemo.Server.Models;
using CosmosDbDemo.Server.Options;
using Microsoft.Extensions.Options;

namespace CosmosDbDemo.Server.Services
{
  public class UserService : IUserService
  {
    private const string _userCollectionName = DatabaseConstants.DatabaseNames.Users;

    private readonly string _databaseName;

    private readonly IDatabaseClient _databaseClient;

    public UserService(IOptions<CosmosDbOptions> options, IDatabaseClient databaseClient)
    {
      _databaseName = options?.Value?.Databases?.CosmosDbDemo;
      if (string.IsNullOrWhiteSpace(_databaseName))
      {
        throw new ArgumentException($"You must supply a CosmosDbDemo database in your CosmosDB options");
      }

      _databaseClient = databaseClient;
    }

    public Task<UserModel> GetUserByEmail(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
      {
        throw new ArgumentException($"Parameter '{nameof(email)}' must have a value");
      }

      var user = _databaseClient.GetDocumentByExpression<UserModel>(_databaseName, _userCollectionName, u => u.Email == email);
      return user;
    }

    public async Task<UserModel> CreateUserIfDoesntExist(UserModel newUser)
    {
      ValidateUserHasEmail(newUser);

      var user = await _databaseClient.GetDocumentByExpression<UserModel>(_databaseName, _userCollectionName, u => u.Email == newUser.Email);
      if (user == null)
      {
        user = await _databaseClient.CreateDocument(_databaseName, _userCollectionName, newUser);
      }

      return user;
    }

    public async Task<UserModel> UpdateUser(UserModel updatedUser)
    {
      ValidateUserHasEmail(updatedUser);

      var user = await _databaseClient.GetDocumentByExpression<UserModel>(_databaseName, _userCollectionName, u => u.Email == updatedUser.Email);

      // update the fields I want to update from the incoming model
      if (!updatedUser.FirstName.IsNullOrWhitespace())
      {
        user.FirstName = updatedUser.FirstName;
      }

      if (!updatedUser.LastName.IsNullOrWhitespace())
      {
        user.LastName = updatedUser.LastName;
      }

      return await _databaseClient.UpdateDocument(_databaseName, _userCollectionName, user);
    }

    public async Task DeleteIfExists(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
      {
        throw new ArgumentException($"Parameter '{nameof(email)}' must have a value");
      }

      var user = await _databaseClient.GetDocumentByExpression<UserModel>(_databaseName, _userCollectionName, u => u.Email == email);
      if (user != null)
      {
        await _databaseClient.DeleteDocument(_databaseName, _userCollectionName, user.Id);
      }
    }

    private bool ValidateUserHasEmail(UserModel user)
    {
      if (string.IsNullOrWhiteSpace(user.Email))
      {
        throw new ArgumentException($"Parameter '{nameof(user)}' must have an '{nameof(user.Email)}' value");
      }

      return true;
    }
  }
}