using AuthenticationWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationWebApi.TokenManager
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthManager(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<JwtSecurityToken> Login(Login model)
        {
            // Implement login logic
            // Ensure to handle user authentication and token generation here
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var token = GetToken(authClaims);
                return token;
            }
            return null;
        }

        public async Task<Response> Register(Register model)
        {
            // Implement user registration logic
            // Handle user creation, role assignment, and response
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return new Response { Status = "Error", Message = "User is already registerd!!" };

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return new Response { Status = "Error", Message = "User Registration failed!!" };

            return new Response { Status = "Success", Message = "User Registration completed:)" };
        }

        public async Task<Response> RegisterAdmin(Register model)
        {
            // Implement admin registration logic
            // Handle user creation, role assignment (admin and user), and response
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return new Response { Status = "Error", Message = "Admin is already registerd!!" };

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return new Response { Status = "Error", Message = "Admin Registration failed!!" };

            if (!await _roleManager.RoleExistsAsync(UserRole.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Admin));

            if (!await _roleManager.RoleExistsAsync(UserRole.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.User));

            if (await _roleManager.RoleExistsAsync(UserRole.Admin))
                await _userManager.AddToRoleAsync(user, UserRole.Admin);

            if (await _roleManager.RoleExistsAsync(UserRole.User))
                await _userManager.AddToRoleAsync(user, UserRole.User);

            return new Response { Status = "Success", Message = "User Registration completed:)" };
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            // Token generation logic
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTAuth:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWTAuth:ValidIssuerURL"],
                audience: _configuration["JWTAuth:ValidAudienceURL"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
    }
}
