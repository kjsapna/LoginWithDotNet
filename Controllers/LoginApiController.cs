using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LoginApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginApiController : ControllerBase
    {

        [HttpPost("Token")]
        public IActionResult Token()
        {
            var header = Request.Headers["Authorization"];
            if (header.ToString().StartsWith("Basic"))
            {
                var credValue = header.ToString().Substring("Basic".Length).Trim();
                var usernameAndPassenc = Encoding.UTF8.GetString(Convert.FromBase64String(credValue));
                var usernameAndPass = usernameAndPassenc.Split(":");
                if (usernameAndPass[0] == "admin" && usernameAndPass[1] == "admin")
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("hsgdjhsgdjhgdoshdgytsnagdhjagdjhd"));
                    var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                    var claimsdata = new[] { new Claim(ClaimTypes.Name, "username") };
                    var token = new JwtSecurityToken(

                    issuer: "mysite.com",
                    audience: "mysite.com",
                    expires: DateTime.Now.AddMinutes(1),
                    claims: claimsdata,
                    signingCredentials: signInCred

                    );

                    var TokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new { token = TokenString });
                }
            }
            return BadRequest("wrong request");
        }

    }
}