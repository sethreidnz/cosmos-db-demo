using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosDbDemo.Server.Models;

namespace CosmosDbDemo.Server.Services
{
  public interface IUserService
  {
    Task<UserModel> GetUser(string email);

    Task<UserModel> CreateUserIfDoesntExist(UserModel userModel);

    Task<UserModel> UpdateUser(UserModel userModel);

    Task<UserModel> DeleteUser(string email);
  }
}