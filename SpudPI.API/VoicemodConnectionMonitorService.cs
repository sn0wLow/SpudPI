namespace SpudPI.API
{
    public class VoicemodConnectionMonitorService : BackgroundService
    {
        private readonly IVoicemodService _voicemodService;
        private readonly ILogger<VoicemodConnectionMonitorService> _logger;
        private readonly IConfiguration _configuration;
        private CancellationTokenSource _onDisconnectCancellationTokenSource;

        public VoicemodConnectionMonitorService(IVoicemodService voicemodService, ILogger<VoicemodConnectionMonitorService> logger, IConfiguration configuration)
        {
            _voicemodService = voicemodService;
            _logger = logger;
            _configuration = configuration;
            _onDisconnectCancellationTokenSource = new CancellationTokenSource();
            _voicemodService.OnDisconnect += VoicemodService_OnDisconnect;
        }

        private void VoicemodService_OnDisconnect(object? sender, EventArgs e)
        {
            _onDisconnectCancellationTokenSource.Cancel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _voicemodService.HeartBeatTestMessage();

                if (!_voicemodService.IsConnected)
                {
                    await AttemptReconnectAsync();
                }
                //else
                //{
                //    _logger.LogInformation("Voicemod API connection is active.");
                //}


                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), _onDisconnectCancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    _onDisconnectCancellationTokenSource.Dispose();
                    _onDisconnectCancellationTokenSource = new CancellationTokenSource();

                    if (!_voicemodService.IsConnected)
                    {
                        await AttemptReconnectAsync();
                    }
                }
            }
        }

        private async Task AttemptReconnectAsync()
        {

            _logger.LogError("Connection to Voicemod API lost");

            while (!_voicemodService.IsConnected)
            {
                try
                {

                    var apiKey = _configuration.GetSection("AppSettings:VoiceModAPIKey").Value;

                    if (string.IsNullOrEmpty(apiKey))
                    {
                        _logger.LogError("API Key for Voicemod not found");
                        await Task.Delay(TimeSpan.FromSeconds(10));
                        continue;
                    }

                    _logger.LogInformation("Attempting to reconnect to Voicemod API");

                    await _voicemodService.ConnectAsync(apiKey);


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            }

            _logger.LogInformation("Successfully reconnected to Voicemod API");
            await Task.Delay(TimeSpan.FromSeconds(10));

        }
    }

}
