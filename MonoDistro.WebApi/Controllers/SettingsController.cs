using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MonoDistro.WebApi.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class SettingsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(UpdateService.Settings);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(UpdateService.Settings.Get(id));
        }

         // GET api/values/5
        [HttpGet("locks")]
        public IActionResult GetLocks(string id)
        {
            return Ok(LockService.Instance.Locks);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Setting setting)
        {
            lock (UpdateService.Settings)
            {
                foreach (var s in UpdateService.Settings)
                {
                    if (s.Key.Equals(setting.Key))
                    {
                        return BadRequest("Your setting is already in that collection.");
                    }

                    Task.Delay(1000).Wait();
                }
                UpdateService.Settings.Add(setting);
            }
            return Ok(setting);
        }
    }
}
