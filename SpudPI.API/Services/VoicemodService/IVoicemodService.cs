using SpudPI.Shared;
using VoicemodAPI;

namespace SpudPI
{
    public interface IVoicemodService
    {
        event EventHandler? OnDisconnect;
        Task ConnectAsync(string apiKey);
        Task HeartBeatTestMessage();
        Task<ServiceResponse<string?>> GetUser();
        Task<ServiceResponse<List<MemeSound>?>> GetAllMemeSounds();
        Task<ServiceResponse<List<Soundboard>>> GetAllSoundboards();
        Task<ServiceResponse<List<MemeSoundBitmap>>> GetAllMemeSoundBitmaps();
        Task<ServiceResponse<List<Soundboard>?>> GetAllSoundboardsIncludingImages();
        Task<ServiceResponse<List<MemeSound>?>> GetAllMemeSoundsIncludingImages();
        Task<ServiceResponse<MemeSoundBitmap?>> GetBitmap(Guid memeID);
        Task<ServiceResponse<bool?>> GetMuteMemeForMeStatus();
        Task<ServiceResponse<bool?>> GetToggleMuteMemeForMe();
        Task<ServiceResponse<bool>> PlayMemeSound(Guid id);
        Task<ServiceResponse<bool>> StopAllMemeSounds();
        bool IsConnected { get; }
    }
}
