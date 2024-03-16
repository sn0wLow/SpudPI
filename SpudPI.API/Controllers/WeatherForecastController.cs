using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpudPI.Shared;

namespace SpudPI.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress;

            if (Request.HttpContext.Connection.RemoteIpAddress is not null)
            {
                if (Request.HttpContext.Connection.RemoteIpAddress.IsIPv4MappedToIPv6)
                {
                    _logger.LogInformation($"WeatherForecast angefragt von: {Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()}");
                }
                else
                {
                    _logger.LogInformation($"WeatherForecast angefragt von: {Request.HttpContext.Connection.RemoteIpAddress}");
                }
            }


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();


        }
    }
}
