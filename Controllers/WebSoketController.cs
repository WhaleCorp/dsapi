using dsapi.DBContext;
using dsapi.SocketController;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace dsapi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WebSoketController:ControllerBase
    {
        private readonly dbcontext _db;
        public WebSoketController(dbcontext _db)
        {
            this._db = _db;
        }
        
    }
}
