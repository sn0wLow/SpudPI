using SpudPI.Shared;
using VoicemodAPI;
using static System.Net.WebRequestMethods;

namespace SpudPI.BlazorClassLibrary
{
    public interface IVoicemodService
    {
        Task<ServiceResponse<bool>> PlayMemeSound(Guid id);
        Task<ServiceResponse<bool>> StopAllMemeSounds();
        Task<ServiceResponse<List<MemeSoundBitmap>>> GetAllMemeSoundBitmaps();
        Task<ServiceResponse<List<Soundboard>>> GetAllSoundboards();
        Task<ServiceResponse<List<Soundboard>?>> GetAllSoundboardsIncludingImages();
        Task<ServiceResponse<List<MemeSound>?>> GetAllMemeSoundsIncludingImages();
        Task<ServiceResponse<bool?>> GetMuteMemeForMeStatus();
        Task<ServiceResponse<bool?>> GetToggleMuteMemeForMe();
    }
}
