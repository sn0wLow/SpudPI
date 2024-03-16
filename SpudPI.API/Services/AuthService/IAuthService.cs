using SpudPI.Shared;

namespace SpudPI.API
{
    public interface IAuthService
    {
        Task<ServiceResponse<string?>> Login(string password);
    }
}
