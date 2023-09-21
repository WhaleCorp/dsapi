using dsapi.DBContext;
using dsapi.Models;
using dsapi.SocketController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dsapi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class MonitorController : ControllerBase
    {
        private readonly dbcontext _db;
        public MonitorController(dbcontext _db)
        {
            this._db = _db;
        }

        [Authorize]
        [HttpPut]
        public IActionResult PutLinkMonitorToUser([FromBody] string monitorCode)
        {
            int id = int.Parse(User.Claims.First(i => i.Type == "UserId").Value);
            try
            {
                var result = _db.Monitor.Single(m => m.Code == monitorCode);
                if (result != null)
                {
                    result.UserId = id;
                    _db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult PostDataToDSPage([FromBody] string data)
        {
            int userId = int.Parse(User.Claims.First(i => i.Type == "UserId").Value);
            try
            {
                string code = _db.Monitor.Where(e => e.UserId == userId).Single().ToString();
                Socket.SendMessage(code, data);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetMonitors()
        {
            int userId = int.Parse(User.Claims.First(i => i.Type == "UserId").Value);
            try
            {
                var monitors = _db.Monitor.Where(e => e.UserId == userId).ToList();
                return Ok(monitors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}