using dsapi.DBContext;
using dsapi.Models;
using dsapi.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserService _userService;
        private readonly dbcontext _db;

        public AuthController(IConfiguration configuration, IUserService userService,dbcontext db)
        {
            _configuration = configuration;
            _userService = userService;
            _db = db;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Auth([FromBody] LoginModel data)
        {
            bool isValid = IsValidUserInformation(data);
            if (isValid)
            {
                var tokenString = GenerateJwtToken(data.Login);
                return Ok(new { Token = tokenString });
            }
            return BadRequest("Please pass the valid Login and Password");
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult GetResult()
        {
            return Ok("API Validated");
        }

        /// <summary>
        /// Generate JWT Token after successful login.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private string GenerateJwtToken(string Login)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", Login) }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [NonAction]
        public bool IsValidUserInformation(LoginModel model)
        {
            var user = _db.User.Join(_db.UserPasswords,
                 user => user.Id,
                 pass => pass.UserId,
                 (user, pass) => new { user, pass }
                 ).Where(u => u.user.Login == model.Login && u.pass.Password == model.Password);

            if (user != null)
            {
                
                return true;
            }
            else return false;
        }
    }
}
