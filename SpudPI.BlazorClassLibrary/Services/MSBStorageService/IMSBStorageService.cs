using VoicemodAPI;

namespace SpudPI.BlazorClassLibrary
{
    public interface IMSBStorageService
    {
        Task SaveBitmapsAsync(IEnumerable<MemeSoundBitmap> memeSoundBitmaps);
        Task<IEnumerable<MemeSoundBitmap>> LoadBitmapsAsync(IEnumerable<Guid> ids);
    }
}
