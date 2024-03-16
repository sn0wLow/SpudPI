using SpudPI.BlazorClassLibrary;
using System.IO;
using System.Text;
using VoicemodAPI;

namespace SpudPI.WPF.Client
{
    public class WPFMSBStorageService : IMSBStorageService
    {
        private readonly string _baseFolderPath;

        public WPFMSBStorageService()
        {
            _baseFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "Resources", "Images", "MSBCache");
            Directory.CreateDirectory(_baseFolderPath);
        }


        public async Task<IEnumerable<MemeSoundBitmap>> LoadBitmapsAsync(IEnumerable<Guid> ids)
        {
            var bitmaps = new List<MemeSoundBitmap>();

            foreach (var id in ids)
            {
                var filePath = Path.Combine(_baseFolderPath, $"{id}.bin");
                if (File.Exists(filePath))
                {
                    var imageBytes = await File.ReadAllBytesAsync(filePath);
                    //var base64Image = Convert.ToBase64String(imageBytes);
                    var base64Image = Encoding.ASCII.GetString(imageBytes);
                    bitmaps.Add(new MemeSoundBitmap
                    {
                        MemeID = id,
                        ImageBase64 = base64Image
                    });
                }
            }

            return bitmaps;
        }

        public async Task SaveBitmapsAsync(IEnumerable<MemeSoundBitmap> memeSoundBitmaps)
        {
            foreach (var bitmap in memeSoundBitmaps)
            {
                var filePath = Path.Combine(_baseFolderPath, $"{bitmap.MemeID}.bin");

                if (!string.IsNullOrEmpty(bitmap.ImageBase64))
                {
                    //var imageBytes = Convert.FromBase64String(bitmap.ImageBase64!);
                    var imageBytes = Encoding.ASCII.GetBytes(bitmap.ImageBase64);
                    await File.WriteAllBytesAsync(filePath, imageBytes);
                }
            }
        }
    }
}
