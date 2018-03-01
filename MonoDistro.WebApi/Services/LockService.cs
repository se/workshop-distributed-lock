using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MonoDistro.WebApi
{
    public class LockService
    {
        private const string LockFolder = "/Users/monocode/Desktop/git/distro/MonoDistro/locks";
        DirectoryInfo _lockDir;
        private LockService()
        {
            _lockDir = new DirectoryInfo(LockFolder);
            if (!_lockDir.Exists)
            {
                _lockDir.Create();
            }
        }

        private static LockService _instance;
        public static LockService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LockService();
                }
                return _instance;
            }
        }

        public List<string> GetLocks()
        {
            var files = _lockDir.GetFiles("*.lock");
            var list = new List<string>();
            foreach (var file in files)
            {
                list.Add(file.Name);
            }
            return list;
        }

        public List<string> Locks
        {
            get
            {
                return GetLocks();
            }
        }

        public bool Lock(string key)
        {
            var seconds = 0;
            while (IsLock(key))
            {
                seconds++;
                if (seconds == 10)
                {
                    return false;
                }
                Task.Delay(1000).Wait();
            }

            File.Create(Path.Combine(_lockDir.FullName, $"{key}.lock"));
            return true;
        }

        public bool IsLock(string key)
        {
            return File.Exists(Path.Combine(_lockDir.FullName, $"{key}.lock"));
        }

        public bool Release(string key)
        {
            File.Delete(Path.Combine(_lockDir.FullName, $"{key}.lock"));
            return true;
        }
    }
}