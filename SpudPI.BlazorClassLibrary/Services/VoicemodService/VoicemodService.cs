using SpudPI.Shared;
using VoicemodAPI;

namespace SpudPI.BlazorClassLibrary
{
    public class VoicemodService : IVoicemodService
    {

        private readonly HttpClient _http;

        public VoicemodService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<List<Soundboard>>> GetAllSoundboards()
        {
            return await ApiHelper.GetAsync<List<Soundboard>>(_http, "api/Voicemod/getAllSoundboards");
        }

        public async Task<ServiceResponse<List<MemeSoundBitmap>>> GetAllMemeSoundBitmaps()
        {
            return await ApiHelper.GetAsync<List<MemeSoundBitmap>>(_http, "api/Voicemod/getAllMemeSoundBitmaps");
        }

        public async Task<ServiceResponse<List<MemeSound>?>> GetAllMemeSoundsIncludingImages()
        {
            return await ApiHelper.GetAsync<List<MemeSound>?>(_http, "api/Voicemod/getAllMemeSoundsIncludingImages");
        }

        public async Task<ServiceResponse<List<Soundboard>?>> GetAllSoundboardsIncludingImages()
        {
            return await ApiHelper.GetAsync<List<Soundboard>?>(_http, "api/Voicemod/getAllSoundboardsIncludingImages");
        }

        public async Task<ServiceResponse<bool?>> GetMuteMemeForMeStatus()
        {
            return await ApiHelper.GetAsync<bool?>(_http, $"api/Voicemod/getMuteMemeForMeStatus");
        }

        public async Task<ServiceResponse<bool?>> GetToggleMuteMemeForMe()
        {
            return await ApiHelper.GetAsync<bool?>(_http, $"api/Voicemod/getToggleMuteMemeForMe");
        }

        public async Task<ServiceResponse<bool>> PlayMemeSound(Guid id)
        {
            return await ApiHelper.PostAsync<string, bool>(_http, $"api/Voicemod/playMemeSound/{id}", null);
        }

        public async Task<ServiceResponse<bool>> StopAllMemeSounds()
        {
            return await ApiHelper.PostAsync<string, bool>(_http, "api/Voicemod/stopAllMemeSounds", null);
        }
    }
}
