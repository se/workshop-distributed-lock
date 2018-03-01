using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MonoDistro.WebApi
{
    public class LockServiceMongo
    {
        IMongoDatabase db;
        IMongoCollection<LockObject> collection;
        private LockServiceMongo()
        {
            db = GetDb();
            collection = db.GetCollection<LockObject>("lock");
        }

        private IMongoDatabase GetDb()
        {
            var settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress("localhost", 27017);

            var client = new MongoClient();

            var db = client.GetDatabase("mono");

            return db;
        }

        private static LockServiceMongo _instance;
        public static LockServiceMongo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LockServiceMongo();
                }
                return _instance;
            }
        }

        public List<string> GetLocks()
        {
            var list = collection.Find(x => true).ToList();
            return list.Select(x => x.Key).ToList();
        }

        public List<string> Locks
        {
            get
            {
                return GetLocks();
            }
        }

        public async Task<bool> LockAsync(string key, int expiration = 5, int wait = 10)
        {
            return await Task.Run(() =>
            {
                return Lock(key, expiration, wait);
            });
        }

        public bool Lock(string key, int expiration = 5, int wait = 10)
        {
            var seconds = 0;
            while (IsLock(key))
            {
                seconds++;
                if (seconds == wait)
                {
                    return false;
                }
                Task.Delay(1000).Wait();
            }

            var lo = new LockObject
            {
                Key = key,
                Expiration = DateTime.Now.AddSeconds(expiration)
            };
            collection.InsertOne(lo);
            return lo.Id != ObjectId.Empty.ToString();
        }

        public async Task<bool> IsLockAsync(string key)
        {
            var lo = await collection.Find(x => x.Key.Equals(key)).FirstOrDefaultAsync();
            if (lo == null)
            {
                return false;
            }
            if (lo.Expiration < DateTime.Now)
            {
                return Release(key);
            }
            return true;
        }

        public bool IsLock(string key)
        {
            var lo = collection.Find(x => x.Key.Equals(key)).FirstOrDefault();
            if (lo == null)
            {
                return false;
            }
            if (lo.Expiration < DateTime.Now)
            {
                return Release(key);
            }
            return true;
        }

        public bool Release(string key)
        {
            var result = collection.DeleteOne(x => x.Key.Equals(key));
            return result.DeletedCount > 0;
        }
    }
}