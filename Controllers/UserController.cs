using dsapi.DBContext;
using dsapi.Models;
using dsapi.Services;
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
        private PasswordHasher<User> hash = new PasswordHasher<User>();
        public UserController(dbcontext db, IUserService userService)
        {
            _db = db;
            _userService = userService;
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateNewUser([FromBody] User user)
        {
            try
            {
                user.Password = hash.HashPassword(user, user.Password);
                _db.User.Add(user);
                _db.SaveChanges();
                return Ok(new
                {
                    data = ""
                });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
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
