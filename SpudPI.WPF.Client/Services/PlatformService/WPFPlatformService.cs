using SpudPI.BlazorClassLibrary;

namespace SpudPI.WPF.Client
{
    public class WpfPlatformService : IPlatformService
    {
        public Platform CurrentPlatform => Platform.WPF;
    }
}
