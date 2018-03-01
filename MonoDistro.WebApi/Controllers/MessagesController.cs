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
    public class MessagesController : ControllerBase
    {
        private static object _guard = new object();
        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(UpdateService.Settings.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]MessageModel model)
        {
            var db = GetDb();
            var messageCollection = db.GetCollection<Message>("message");

            var user = await GetUser(model);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var message = new Message
            {
                UserId = user.Id,
                Content = model.Message,
            };

            await messageCollection.InsertOneAsync(message);

            return Ok(new
            {
                User = user,
                Message = message
            });
        }

        private async Task<User> GetUser(MessageModel model)
        {
            var lockKey = $"USER_GET_{model.UserName}";
            // On File System
            //if (!LockService.Instance.Lock(lockKey))
            // On Mongo Sync
            /*if (!LockServiceMongo.Instance.Lock(lockKey, 15, 20))
            {
                return null;
            }*/

            // On Mongo Async
            await LockServiceMongo.Instance.LockAsync(lockKey, 15, 20);
            if (await LockServiceMongo.Instance.IsLockAsync(lockKey))
            {
                return null;
            }

            User user = null;
            var db = GetDb();
            var usersCollection = db.GetCollection<User>("user");
            lock (_guard)
            {
                user = usersCollection.Find(x => x.UserName.Equals(model.UserName)).FirstOrDefault();
                if (user == null)
                {
                    user = new User
                    {
                        UserName = model.UserName,
                        Name = model.Name,
                        SurName = model.SurName
                    };

                    UpdateService.UpdateUserCount();
                    usersCollection.InsertOne(user);
                }
            }

            // On File System
            // LockService.Instance.Release(lockKey);
            // On MongoDb
            LockServiceMongo.Instance.Release(lockKey);
            return await Task.FromResult(user);
        }

        private IMongoDatabase GetDb()
        {
            var settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress("localhost", 27017);

            var client = new MongoClient();

            var db = client.GetDatabase("mono");

            return db;
        }
    }
}
