using CosmosDbDemo.Server.Models;
using CosmosDbDemo.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace CosmosDbDemo.Server.Controllers
{
  [Route("api/[controller]")]
  public class ProfileController : Controller
  {
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
      _userService = userService;
    }

    // GET api/profile/me
    [HttpGet]
    public IActionResult GetCurrentUser()
    {
      var userEmail = "test@test.com";
      var user = _userService.GetUser(userEmail);
      if (user == null)
      {
        return Unauthorized();
      }

      return Ok(user);
    }

    // POST api/profile
    [HttpPost]
    public IActionResult Post([FromBody]UserModel updatedUser)
    {
      if (string.IsNullOrWhiteSpace(updatedUser.Email))
      {
        return BadRequest();
      }

      var users = _userService.CreateUserIfDoesntExist(updatedUser);
      return Ok(users);
    }

    // PUT api/profile/test@email.com
    [HttpPut("{email}")]
    public IActionResult Put(string email, [FromBody]UserModel updatedUser)
    {
      var user = _userService.GetUser(email);
      if (user == null)
      {
        // TODO
      }

      var users = _userService.UpdateUser(updatedUser);
      return Ok(user);
    }

    // DELETE api/profile/5
    [HttpDelete("{email}")]
    public IActionResult Delete(string email)
    {
      _userService.DeleteUser(email);
      return Ok();
    }
  }
}