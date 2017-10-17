using System.Threading.Tasks;
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
    [HttpGet("{email}")]
    public async Task<IActionResult> GetUser(string email)
    {
      var user = await _userService.GetUserByEmail(email);
      if (user == null)
      {
        return NotFound(email);
      }

      return Ok(user);
    }

    // POST api/profile
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]UserModel updatedUser)
    {
      var user = await _userService.CreateUserIfDoesntExist(updatedUser);
      return Ok(user);
    }

    // PUT api/profile/test@email.com
    [HttpPut("{email}")]
    public async Task<IActionResult> Put(string email, [FromBody]UserModel updatedUser)
    {
      updatedUser = await _userService.UpdateUser(updatedUser);
      return Ok(updatedUser);
    }

    // DELETE api/profile/5
    [HttpDelete("{email}")]
    public async Task<IActionResult> Delete(string email)
    {
      await _userService.DeleteIfExists(email);
      return Ok();
    }
  }
}