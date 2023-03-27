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

        [HttpPut]
        public IActionResult LinkMonitorToUser([FromBody]UserIdMonitorName obj)
        {
            try
            {
                var result = _db.Monitor.SingleOrDefault(m=>m.MonitorName==obj.MonitorName);
                if (result != null)
                {
                    result.UserId = obj.UserId;
                    _db.SaveChanges();
                }
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult PostDataToDSPage([FromBody] MonitorMessage mm)
        {
            try
            {
                Socket.SendMessage(mm.Guid, mm.Data);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }    
        }

        [HttpGet]
        public IActionResult TestGet()
        {
            return Ok();
        }
    }
}