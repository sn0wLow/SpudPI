using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SpudPI.Services
{
    public class VoicemodConnectionHealthService : IHealthCheck
    {
        private readonly IVoicemodService _voicemodService;
        private readonly ILogger<VoicemodConnectionHealthService> _logger;

        public VoicemodConnectionHealthService(IVoicemodService voicemodService, ILogger<VoicemodConnectionHealthService> logger)
        {
            _voicemodService = voicemodService;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            HealthCheckResult result;



            if (_voicemodService.IsConnected)
            {
                result = HealthCheckResult.Healthy("Connected to Voicemod API");
            }
            else
            {
                result = HealthCheckResult.Unhealthy("Disconnected from Voicemod API");
            }

            return result;
        }
    }
}
