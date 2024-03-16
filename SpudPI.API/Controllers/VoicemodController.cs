using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpudPI.Shared;
using VoicemodAPI;

namespace SpudPI.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class VoicemodController : ControllerBase
    {
        private readonly IVoicemodService _voicemodService;
        private readonly ILogger<VoicemodController> _logger;

        public VoicemodController(IVoicemodService voicemodService, ILogger<VoicemodController> logger)
        {
            _voicemodService = voicemodService;
            _logger = logger;
        }

        private void LogIP(string action)
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress;

            if (Request.HttpContext.Connection.RemoteIpAddress is not null)
            {
                if (Request.HttpContext.Connection.RemoteIpAddress.IsIPv4MappedToIPv6)
                {
                    _logger.LogInformation($"{action} requested from: {Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()}");
                }
                else
                {
                    _logger.LogInformation($"{action} requested from: {Request.HttpContext.Connection.RemoteIpAddress}");
                }
            }
        }

        [HttpGet("getUser")]
        public async Task<ServiceResponse<string?>> GetUser()
        {
            LogIP("User");

            return await _voicemodService.GetUser();
        }

        [HttpGet("getAllMemeSoundBitmaps")]
        public async Task<ServiceResponse<List<MemeSoundBitmap>>> GetAllMemeSoundBitmaps()
        {
            LogIP("All MemeSound Bitmaps");

            return await _voicemodService.GetAllMemeSoundBitmaps();
        }

        [HttpGet("getAllMemeSounds")]
        public async Task<ServiceResponse<List<MemeSound>?>> GetAllMemeSounds()
        {
            LogIP("All MemeSounds");

            return await _voicemodService.GetAllMemeSounds();
        }

        [HttpGet("getAllSoundboards")]
        public async Task<ServiceResponse<List<Soundboard>>> GetAllSoundboards()
        {
            LogIP("All Soundboards");

            return await _voicemodService.GetAllSoundboards();
        }

        [HttpGet("getAllMemeSoundsIncludingImages")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public async Task<ServiceResponse<List<MemeSound>?>> GetAllMemeSoundsIncludingImages()
        {
            LogIP("All MemeSounds including Images");

            return await _voicemodService.GetAllMemeSoundsIncludingImages();
        }

        [HttpGet("getAllSoundboardsIncludingImages")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public async Task<ServiceResponse<List<Soundboard>?>> GetAllSoundboardsIncludingImages()
        {
            LogIP("All Soundboards including Images");

            return await _voicemodService.GetAllSoundboardsIncludingImages();
        }

        [HttpGet("getBitmap/{memeID}")]
        public async Task<ServiceResponse<MemeSoundBitmap?>> GetBitmap(Guid memeID)
        {
            LogIP($"Bitmap {memeID}");

            return await _voicemodService.GetBitmap(memeID);
        }

        [HttpGet("getMuteMemeForMeStatus")]
        public async Task<ServiceResponse<bool?>> GetMuteMemeForMeStatus()
        {
            LogIP("Get Mute Meme For Me Status");

            return await _voicemodService.GetMuteMemeForMeStatus();
        }

        [HttpGet("getToggleMuteMemeForMe")]
        public async Task<ServiceResponse<bool?>> GetToggleMuteMemeForMe()
        {
            LogIP("Toggle Mute Meme For Me Status");

            return await _voicemodService.GetToggleMuteMemeForMe();
        }

        [HttpPost("playMemeSound/{id}")]
        public async Task<ServiceResponse<bool>> PlaySound(Guid id)
        {
            LogIP($"Play MemeSound {id}");

            return await _voicemodService.PlayMemeSound(id);
        }

        [HttpPost("stopAllMemeSounds")]
        public async Task<ServiceResponse<bool>> StopAllMemeSounds()
        {
            LogIP("Stop all Meme Sounds");

            return await _voicemodService.StopAllMemeSounds();
        }
    }
}
