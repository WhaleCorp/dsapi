using dsapi.DBContext;
using dsapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;

namespace dsapi.Middlware
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JWTMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachAccountToContext(context,userService, token);
            await _next(context);
        }

        private void AttachAccountToContext(HttpContext context, IUserService userService, string token)
        {

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = _configuration["Jwt:Key"];
                IPrincipal principal = tokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateLifetime = false, 
                    ValidateAudience = true, 
                    ValidateIssuer = true,   
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);

            if(userId!= 0)
                context.Items["User"] = userService.GetById(userId);

        }


    }
}

