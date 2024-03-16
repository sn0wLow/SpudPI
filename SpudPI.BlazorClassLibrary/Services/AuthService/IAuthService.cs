using SpudPI.Shared;

namespace SpudPI.BlazorClassLibrary
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Login(LoginDTO request);
        Task<bool> IsUserAuthenticated();
    }
}
