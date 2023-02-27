using dsapi.SocketController;
using Microsoft.AspNetCore.Mvc;

namespace dsapi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ControllerController : ControllerBase
    {
        public class PostRq
        {
            public string guid { get; set; }
            public string data { get; set; }
        }

        public ControllerController() { }

        [HttpPost]
        public async Task<IActionResult> PostDataToDSPage([FromBody] PostRq pq)
        {
            try
            {
                Socket.SendMessage(pq.guid, pq.data);
                Console.WriteLine(pq);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }    
        }
    }
}