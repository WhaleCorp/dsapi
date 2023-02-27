using dsapi.SocketController;
using Microsoft.AspNetCore.Mvc;

namespace dsapi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ConnectionsController:ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllConnected()
        {
            Socket.CleanSoket();
            return Ok(Socket.sockets.Keys);
        }
    }
}
