using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosDbDemo.Server.Clients;
using CosmosDbDemo.Server.Constants;
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

    public Task<UserModel> GetUser(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
      {
        throw new ArgumentException($"Parameter '{nameof(email)}' must have a value");
      }

      var user = _databaseClient.GetDocumentByExpression<UserModel>(_databaseName, _userCollectionName, u => u.Email == email);
      return user;
    }

    public Task<UserModel> CreateUserIfDoesntExist(UserModel newUser)
    {
      ValidateUserHasEmail(newUser);

      var user = _databaseClient.GetDocumentByExpression<UserModel>(_databaseName, _userCollectionName, u => u.Email == newUser.Email);
      if (user == null)
      {
        user = _databaseClient.CreateDocument(_databaseName, _userCollectionName, newUser);
      }

      return user;
    }

    public Task<UserModel> UpdateUser(UserModel userModel)
    {
      ValidateUserHasEmail(userModel);

      var user = _databaseClient.GetDocumentByExpression<UserModel>(_databaseName, _userCollectionName, u => u.Email == userModel.Email);
      if (user == null)
      {
      }

      return user;
    }

    public Task<UserModel> DeleteUser(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
      {
        throw new ArgumentException($"Parameter '{nameof(email)}' must have a value");
      }

      throw new NotImplementedException();
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