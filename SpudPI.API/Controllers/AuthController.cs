using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpudPI.Shared;

namespace SpudPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string?>>> Login(LoginDTO request)
        {
            var response = await _authService.Login(request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }


            return Ok(response);
        }
    }
}
