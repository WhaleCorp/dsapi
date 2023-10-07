using dsapi.DBContext;
using dsapi.Models;
using dsapi.SocketController;
using dsapi.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dsapi.Controllers
{
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
                    _db.MonitorData.Add(new MonitorData(id, result.Code));
                    _db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        
        [HttpPost]
        public IActionResult PostDataToDSPage([FromBody] string data)
        {
            //Replace " to ' !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            try
            {
                //var result = _db.MonitorData.Where(e => e.Code == code).Single();
                //if (result != null)
                //{
                //    result.Data = data;
                //    _db.SaveChanges();
                //}
                //Socket.SendMessageAsync(code, 200.ToString());
                return Ok(new { data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        async public Task<IActionResult> GetData(string code)
        {
            try
            {
                string data = await Task.Run(() => _db.MonitorData.Where(e => e.Code == code).Single().Data.ToString());
                return Ok(new { Data = data });
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