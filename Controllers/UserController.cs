using System.Threading;
using dsapi.DBContext;
using dsapi.Services;
using dsapi.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dsapi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly dbcontext _db;
        private readonly IUserService _userService;
        public UserController(dbcontext db, IUserService userService)
        {
            _db = db;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet]
        public object CheckLogins(string login)
        {
            if (_db.User.Any(u => u.Login == login))
                return new { available = false, login = "" };
            else
                return new { available = true, login }; ;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllUser()
        {
            try
            {
            string roleName =User.Claims.First(i => i.Type == "Role").Value;
            if (roleName != "Admin") return BadRequest("Need admin token");
                var users = _db.Monitor.GroupBy(n => n.UserId,n=>n.Code,(userId,codes) => new
                {
                    Key = userId,
                    Count = codes.Count(),
                    Codes = codes.ToList()
                    
                }).Join(
                    _db.User,
                    monitor => monitor.Key,
                    user => user.Id,
                    (monitor, user) => new
                    {
                        user.Id,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.PhoneNumber,
                        monitor.Count,
                        monitor.Codes
                    });

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
