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

        
    }
}
