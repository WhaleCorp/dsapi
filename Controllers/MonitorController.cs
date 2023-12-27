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
        private bool IsAds { get; set; }
        private bool IsData { get; set; }
        private Queue<string> codeQueue = new Queue<string>();
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
                    if (_db.MonitorData.FirstOrDefault(n => n.Code.Equals(code)) == null)
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
        public IActionResult PutUpdateOrientation([FromBody]OrientationModel model)
        {
            try
            {
                var monitor = _db.Monitor.Single(n => n.Code == model.Code);
                if(monitor !=null)
                {
                    monitor.Orientation = model.Orientation;
                    _db.SaveChanges();
                }    
                return Ok("Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult PostAddAds([FromBody]MonitorAds data)
        {
            try
            {
                var ads = _db.Ads.SingleOrDefault(a => a.Orientation == data.Orientation);
                if (ads != null)
                    ads.Photo = data.Ads;
                else
                    _db.Ads.Add(new Ads(data.Orientation, data.Ads));
                _db.SaveChanges();
                IsAds = true;
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
                    IsData = true;
                    codeQueue.Enqueue(data.Code);
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

        [AllowAnonymous]
        [HttpGet]
        async public Task<IActionResult> GetAds(string code)
        {
            try
            {
                var orientation = await Task.Run(() => _db.Monitor.Single(m => m.Code == code).Orientation);
                if (orientation != null)
                {
                    var data = await Task.Run(() => _db.Ads.SingleOrDefault(a => a.Orientation == orientation).Photo);
                    return Ok(new { Data = data });
                }
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult CheckUpdates(string code)
        {
            try
            {

                if (IsData && codeQueue.Dequeue() == code)
                    return Ok(new { code = 222 });
                if (IsAds)
                    return Ok(new { code = 111 });
                return Ok(new { code = 000 });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}