using dsapi.DBContext;
using dsapi.Models;
using dsapi.Services;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost]
        public IActionResult CreateNewUser([FromBody] CreateUserModel createUserModel)
        {
            try
            {
                _db.User.Add(createUserModel.User);
                var user = _db.User.Where(u => u.Login == createUserModel.User.Login).FirstOrDefault();
                createUserModel.UserPassword.UserId = user.Id;
                _db.UserPasswords.Add(createUserModel.UserPassword);
                _db.SaveChanges();
                return Ok(new
                {
                    data = "User Added"
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
