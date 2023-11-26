using dsapi.DBContext;
using dsapi.Models;
using dsapi.SocketController;
using dsapi.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        [HttpGet]
        public IActionResult PutLinkMonitorToUser(string code)
        {
            int id = int.Parse(User.Claims.First(i => i.Type == "UserId").Value);
            try
            {
                var result = _db.Monitor.SingleOrDefault(m => m.Code.Equals (code));
                if (result != null)
                {
                    result.UserId = id;
                    if (_db.MonitorData.First(n => n.Code.Equals(code)) != null)
                        _db.MonitorData.Add(new MonitorData(id, result.Code));
                    _db.SaveChanges();
                    return Ok("Success");
                }
                return Ok("Monitor doesn't exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult PutUpdateOrientation([FromBody]string orientation,string code)
        {
            try
            {
                var monitor = _db.Monitor.Single(n => n.Code == code);
                if(monitor !=null)
                {
                    monitor.Orientation = orientation;
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
        public IActionResult PostAdsToMonitors([FromBody]MonitorAds data)
        {
            try
            {
                var monitors = _db.Monitor.Where(n => n.Orientation.Equals(data.Orientation)).Select(n => n.Code).ToList();
                var update = _db.MonitorData.Where(n => monitors.Contains(n.Code));
                if (update != null)
                {
                    foreach (var i in update)
                        i.Ads = data.Ads;
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
        public IActionResult PostDataToDSPage([FromBody] MonitorMessage data)
        {
            //Replace " to ' !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            try
            {
                
                var result = _db.MonitorData.Single(e => e.Code.Equals(data.Code));
                if (result != null)
                {
                    result.Data = data.Data;
                    result.RawData = data.RawData;
                    _db.SaveChanges();
                }
                else
                    return Ok("Monitor doesn't exist");
                Socket.SendMessageAsync(data.Code, 200.ToString());
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
                var data = await Task.Run(() => _db.MonitorData.Where(e => e.Code == code).SingleOrDefault());
                if (data != null)
                    return Ok(new { Data = data.Data });
                return Ok(new { Data = "Data doesn't exist" });
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
            try
            {
                int userId = int.Parse(User.Claims.First(i => i.Type == "UserId").Value);
                var monitors = _db.Monitor.Where(e => e.UserId == userId).ToList();
                return Ok(monitors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [Authorize]
        [HttpGet]
        public IActionResult GetMonitor(int userId)
        {
            try
            {
                string roleName = User.Claims.First(i => i.Type == "Role").Value;
                if (roleName != "Admin") return BadRequest("Need admin token");
                var monitors = _db.Monitor.Where(n => n.UserId == userId).ToList();
                return Ok(monitors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}