using System.Net.WebSockets;

namespace SpudPI.API
{
    public class VoicemodConnectionInitializer : IHostedService
    {
        private readonly IVoicemodService _voicemodService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<VoicemodConnectionInitializer> _logger;


        public VoicemodConnectionInitializer(IVoicemodService voicemodService, IConfiguration configuration, ILogger<VoicemodConnectionInitializer> logger)
        {
            _voicemodService = voicemodService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting connection attempt to Voicemod API");

            while (!_voicemodService.IsConnected)
            {
                try
                {
                    var apiKey = _configuration.GetSection("AppSettings:VoiceModAPIKey").Value;
                    if (string.IsNullOrEmpty(apiKey))
                    {
                        _logger.LogError("API Key for Voicemod not found");
                        await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                        continue;
                    }

                    await _voicemodService.ConnectAsync(apiKey);
                }
                catch (WebSocketException ex)
                {

                    _logger.LogError($"Connecting to Voicemod API failed: {ex.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{ex.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                }
            }

            _logger.LogInformation($"Successfully connected to Voicemod API");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
