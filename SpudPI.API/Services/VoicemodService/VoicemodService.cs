using SpudPI.Shared;
using VoicemodAPI;

namespace SpudPI
{
    public class VoicemodService : IVoicemodService
    {
        public event EventHandler? OnDisconnect;

        private readonly VoiceModAPI _voiceModAPI;
        private readonly ILogger<VoicemodService> _logger;
        private readonly IConfiguration _configuration;

        public VoicemodService(ILogger<VoicemodService> logger, IConfiguration configuration)
        {
            _voiceModAPI = new VoiceModAPI();
            _logger = logger;
            _configuration = configuration;
            _voiceModAPI.OnDisconnect += (_, __) => { OnDisconnect?.Invoke(null, new EventArgs()); };
        }

        public async Task ConnectAsync(string apiKey)
        {
            if (!IsConnected)
            {
                await _voiceModAPI.ConnectAsync(apiKey);
            }
        }

        public async Task HeartBeatTestMessage()
        {
            await _voiceModAPI.HeartBeatTestMessage();
        }

        public async Task<ServiceResponse<string?>> GetUser()
        {
            try
            {
                var getUserResponse = await _voiceModAPI.GetUserResponse();

                return new ServiceResponse<string?>
                {
                    Success = true,
                    Data = getUserResponse?.ActionObject?.UserID,
                    Message = ""
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);

                return new ServiceResponse<string?>
                {
                    Success = false,
                    Data = null,
                    Message = $"Unable to get all User: Internal Error connecting to Voicemod API"
                };
            }

        }

        public async Task<ServiceResponse<List<MemeSound>?>> GetAllMemeSounds()
        {
            try
            {
                var getAllMemeSoundsResponse = await _voiceModAPI.GetAllMemeSoundsResponse();

                return new ServiceResponse<List<MemeSound>?>
                {
                    Success = true,
                    Data = getAllMemeSoundsResponse?.ActionObject?.ListOfMemes,
                    Message = ""
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);

                return new ServiceResponse<List<MemeSound>?>
                {
                    Success = false,
                    Data = null,
                    Message = $"Unable to get all MemeSounds: Internal Error connecting to Voicemod API"
                };
            }

        }

        public async Task<ServiceResponse<List<Soundboard>>> GetAllSoundboards()
        {
            try
            {
                var getAllSoundboards = await _voiceModAPI.GetAllSoundboards();

                return new ServiceResponse<List<Soundboard>>
                {
                    Success = true,
                    Data = getAllSoundboards,
                    Message = ""
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);

                return new ServiceResponse<List<Soundboard>>
                {
                    Success = false,
                    Data = null,
                    Message = $"Unable to get all Soundboards: Internal Error connecting to Voicemod API"
                };
            }

        }

        public async Task<ServiceResponse<List<MemeSoundBitmap>>> GetAllMemeSoundBitmaps()
        {
            try
            {
                var memeSoundBitmaps = await _voiceModAPI.GetAllMemeSoundBitmaps();

                return new ServiceResponse<List<MemeSoundBitmap>>
                {
                    Success = true,
                    Data = memeSoundBitmaps,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new ServiceResponse<List<MemeSoundBitmap>>
                {
                    Success = false,
                    Data = null,
                    Message = $"Unable to get all MemeSound Images: Internal Error connecting to Voicemod API"
                };
            }
        }

        public async Task<ServiceResponse<List<Soundboard>?>> GetAllSoundboardsIncludingImages()
        {
            try
            {
                var getAllSoundboardsResponse = await _voiceModAPI.GetAllSoundboardsIncludingImagesResponse();

                return new ServiceResponse<List<Soundboard>?>
                {
                    Success = true,
                    Data = getAllSoundboardsResponse?.ActionObject?.Soundboards,
                    Message = ""
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);

                return new ServiceResponse<List<Soundboard>?>
                {
                    Success = false,
                    Data = null,
                    Message = $"Unable to get all Soundboards: Internal Error connecting to Voicemod API"
                };
            }

        }

        public async Task<ServiceResponse<List<MemeSound>?>> GetAllMemeSoundsIncludingImages()
        {
            try
            {
                var getAllSoundboardsResponse = await _voiceModAPI.GetAllMemeSoundsIncludingImagesResponse();

                return new ServiceResponse<List<MemeSound>?>
                {
                    Success = true,
                    Data = getAllSoundboardsResponse?.ActionObject?.ListOfMemes,
                    Message = ""
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);

                return new ServiceResponse<List<MemeSound>?>
                {
                    Success = false,
                    Data = null,
                    Message = $"Unable to get all MemeSounds: Internal Error connecting to Voicemod API"
                };
            }

        }

        public async Task<ServiceResponse<MemeSoundBitmap?>> GetBitmap(Guid memeID)
        {
            try
            {
                var getAllSoundboardsResponse = await _voiceModAPI.GetBitmapResponse(memeID);

                return new ServiceResponse<MemeSoundBitmap?>
                {
                    Success = true,
                    Data = getAllSoundboardsResponse?.ActionObject?.MemeSoundBitmap,
                    Message = ""
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);

                return new ServiceResponse<MemeSoundBitmap?>
                {
                    Success = false,
                    Data = null,
                    Message = $"Unable to get Bitmap: Internal Error connecting to Voicemod API"
                };
            }

        }

        public async Task<ServiceResponse<bool?>> GetMuteMemeForMeStatus()
        {
            try
            {
                var getMuteMemeForMeStatusResponse = await _voiceModAPI.GetMuteMemeForMeStatusResponse();

                return new ServiceResponse<bool?>
                {
                    Success = true,
                    Data = getMuteMemeForMeStatusResponse?.ActionObject?.Value,
                    Message = ""
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);

                return new ServiceResponse<bool?>
                {
                    Success = false,
                    Data = false,
                    Message = "Unable to play Get Mute Meme For Me Status"
                };
            }
        }

        public async Task<ServiceResponse<bool?>> GetToggleMuteMemeForMe()
        {
            try
            {
                var getToggleMuteMemeForMeResponse = await _voiceModAPI.GetToggleMuteMemeForMeResponse();

                return new ServiceResponse<bool?>
                {
                    Success = true,
                    Data = getToggleMuteMemeForMeResponse?.ActionObject?.Value,
                    Message = ""
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);

                return new ServiceResponse<bool?>
                {
                    Success = false,
                    Data = false,
                    Message = "Unable to toggle Get Mute Meme For Me Status"
                };
            }
        }

        public async Task<ServiceResponse<bool>> PlayMemeSound(Guid id)
        {
            try
            {
                await _voiceModAPI.PlayMemeSound(id);

                return new ServiceResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);

                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Unable to play MemeSound"
                };
            }
        }

        public async Task<ServiceResponse<bool>> StopAllMemeSounds()
        {
            try
            {
                await _voiceModAPI.StopAllMemeSounds();

                return new ServiceResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);

                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = $"Unable to stop all MemeSounds: Internal Error connecting to Voicemod API"
                };
            }
        }

        public bool IsConnected => _voiceModAPI.IsConnected;
    }
}
