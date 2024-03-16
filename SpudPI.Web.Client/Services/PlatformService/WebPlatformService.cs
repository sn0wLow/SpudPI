using SpudPI.BlazorClassLibrary;

namespace SpudPI.Web.Client
{
    public class WebPlatformService : IPlatformService
    {
        public Platform CurrentPlatform => Platform.Web;
    }
}
