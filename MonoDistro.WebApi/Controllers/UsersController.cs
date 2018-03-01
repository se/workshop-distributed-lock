using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace MonoDistro.WebApi.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class UsersController : ControllerBase
    {
        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(UpdateService.Settings.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User user)
        {
            var settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress("localhost", 27017);

            var client = new MongoClient();

            var db = client.GetDatabase("mono");
            var usersCollection = db.GetCollection<User>("user");

            if (await usersCollection.Find(x => x.UserName.Equals(user.UserName)).AnyAsync())
            {
                return BadRequest("This user is already in our database.");
            }

            UpdateService.UpdateUserCount();

            await usersCollection.InsertOneAsync(user);

            return Ok(user);
        }
    }
}
