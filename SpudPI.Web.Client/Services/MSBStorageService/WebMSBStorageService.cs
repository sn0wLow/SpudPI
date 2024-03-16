using Microsoft.JSInterop;
using SpudPI.BlazorClassLibrary;
using System.Text.Json;
using VoicemodAPI;

namespace SpudPI.Web
{
    public class WebMSBStorageService : IMSBStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public WebMSBStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        public async Task<IEnumerable<MemeSoundBitmap>> LoadBitmapsAsync(IEnumerable<Guid> ids)
        {
            var keyStrings = ids.Select(k => k.ToString()).ToList();
            var jsonResult = await _jsRuntime.InvokeAsync<IEnumerable<MemeSoundBitmap>>("loadMemeSoundBitmapsFromIndexedDB", keyStrings);
            // var memeSoundBitmaps = JsonSerializer.Deserialize<List<MemeSoundBitmap>>(jsonResult);
            return jsonResult!;
        }

        public async Task SaveBitmapsAsync(IEnumerable<MemeSoundBitmap> memeSoundBitmaps)
        {
            var json = JsonSerializer.Serialize(memeSoundBitmaps);
            await _jsRuntime.InvokeVoidAsync("saveMemeSoundBitmapsToIndexedDB", json);
        }
    }
}
