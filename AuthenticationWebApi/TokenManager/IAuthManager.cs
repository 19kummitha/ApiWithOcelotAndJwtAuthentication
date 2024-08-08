using AuthenticationWebApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationWebApi.TokenManager
{
    public interface IAuthManager
    {
        Task<JwtSecurityToken> Login(Login model);
        Task<Response> Register(Register model);
        Task<Response> RegisterAdmin(Register model);
    }
}
