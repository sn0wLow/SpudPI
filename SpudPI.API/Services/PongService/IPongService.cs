using SpudPI.Shared;

namespace SpudPI.Services
{
    public interface IPongService
    {
        Task<ServiceResponse<Guid>> Pong(Guid guid);
    }
}
