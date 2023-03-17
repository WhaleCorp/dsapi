using dsapi.Models;
using dsapi.SocketController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dsapi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ControllerController : ControllerBase
    {
        public ControllerController() { }

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