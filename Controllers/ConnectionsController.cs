using dsapi.SocketController;
using Microsoft.AspNetCore.Mvc;

namespace dsapi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ConnectionsController:ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllConnected()
        {
            Socket.CleanSoket();
            return Ok(Socket.sockets.Keys);
        }
    }
}
