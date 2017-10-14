using System.Collections.Generic;
using CosmosDbDemo.Server.Clients;
using Microsoft.AspNetCore.Mvc;

namespace CosmosDbDemo.Server.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IDatabaseClient _databaseClient;

        public UserController(IDatabaseClient databaseClient)
        {
            _databaseClient = databaseClient;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}