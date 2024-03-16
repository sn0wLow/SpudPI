using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SpudPI.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpudPI.API
{
    public class AuthService : IAuthService
    {
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IPasswordService passwordService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _passwordService = passwordService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<string?>> Login(string password)
        {
            var response = new ServiceResponse<string?>();

            if (!BCrypt.Net.BCrypt.Verify(password, _passwordService.HashedPassword))
            {
                response.Data = null;
                response.Success = false;
                response.Message = "Incorrect Password";
            }
            else
            {
                response.Data = CreateToken();
                response.Success = true;
            }

            return response;
        }

        private string CreateToken()
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "user"),
            };

            var key = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken
            (
               claims: claims,
               expires: DateTime.Now.AddDays(5),
               signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
