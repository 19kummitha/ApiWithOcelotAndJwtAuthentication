using AuthenticationWebApi.Models;
using AuthenticationWebApi.TokenManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthenticationController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var result = await _authManager.Login(model);
            if (result == null) { return Ok("Something went wrong!!.. Please try again!!"); }
            return (Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(result),
                Generated_on = DateTime.Now,
                Expairation = result.ValidTo
            }));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            return Ok(await _authManager.Register(model));
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] Register model)
        {
            return Ok(await _authManager.RegisterAdmin(model));
        }

    }
}
