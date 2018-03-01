using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonoDistro.WebApi
{
    public static class UpdateService
    {
        private static int _counter = 0;
        public static SettingsCollection Settings { get; set; } = new SettingsCollection();
        public static int UserCount { get; set; } = 0;

        public static void Initialize()
        {
            Settings.Add("StartTime", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}");
            Settings.Add("UpdateTime", $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}");
            Settings.Add("Counter", "24");
            Settings.Add("ApiUrl", "https://api.monosay.com/v1/");

            var task = new Task(async () => { await Update(); });
            task.Start();
        }

        private async static Task Update()
        {
            while (true)
            {
                Settings["UpdateTime"].Value = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}";

                lock (Settings)
                {
                    Settings.Remove("Counter");
                    Settings.Add("Counter", _counter++.ToString());
                }

                await Task.Delay(1000);
            }
        }

        public static void UpdateUserCount()
        {
            UserCount++;
            Task.Delay(3000).Wait();
        }
    }
}