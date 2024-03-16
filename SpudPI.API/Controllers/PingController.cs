using Microsoft.AspNetCore.Mvc;
using SpudPI.Services;
using SpudPI.Shared;

namespace SpudPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly IPongService _pongService;
        private readonly ILogger<PongService> _logger;

        public PingController(IPongService pongService, ILogger<PongService> logger)
        {
            _pongService = pongService;
            _logger = logger;
        }

        [HttpGet("{guid}")]
        public async Task<ServiceResponse<Guid>> Ping(Guid guid)
        {
            return await _pongService.Pong(guid);
        }
    }
}
