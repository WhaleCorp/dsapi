using dsapi.DBContext;
using dsapi.Models;
using dsapi.Services;
using dsapi.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dsapi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly dbcontext _db;
        private readonly IUserService _userService;
        private PasswordHasher<User> hash = new PasswordHasher<User>();

        public AuthController(IConfiguration configuration, dbcontext db, IUserService service)
        {
            _configuration = configuration;
            _db = db;
            _userService = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SignIn([FromBody] LoginModel data)
        {
            IdRoleModel model = _userService.IsValidUserInformation(data);
            if (model.Id != 00)
            {
                var tokenString = GenerateJwtToken(model.Id,model.RoleName);
                return Ok(new {role=model.RoleName, Token = tokenString } );
            }
            return BadRequest("Please pass the valid Login and Password");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SignUp([FromBody] User user)
        {
            try
            {
                user.Password = hash.HashPassword(user, user.Password);
                user.RoleId = 2;
                _db.User.Add(user);
                _db.SaveChanges();
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        

        [Authorize]
        [HttpGet]
        public IActionResult SignOut()
        {
            int id = int.Parse(User.Claims.First(i => i.Type == "UserId").Value);
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult CheckUserRole()
        {
            try
            {
                string roleName = User.Claims.First(i => i.Type == "Role").Value;
                if (roleName == "Admin") return Ok("Admin");
                else if (roleName == "User") return Ok("User");
                else if (roleName == "Manager") return Ok("Manager");
                return BadRequest("Something went wrong");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        public IActionResult getTest()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetMonitorCode()
        {
            int len = 10;
            StringBuilder code = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i <= len; i++)
            {
                code.Append(GetRandomCharacter(random));
            }
            _db.Monitor.Add(new Tables.Monitor(code.ToString(),"Horizontal"));
            _db.SaveChanges();
            return Ok(new { code=code.ToString() });

            char GetRandomCharacter(Random rnd)
            {
                string text = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                int index = rnd.Next(text.Length);
                return text[index];
            }
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult Result()
        {
            return Ok("API Validated");
        }

        /// <summary>
        /// Generate JWT Token after successful login.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private string GenerateJwtToken(int id,string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("UserId", id.ToString()),new Claim("Role",role) }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
