using SpudPI.Shared;

namespace SpudPI.Services
{
    public class PongService : IPongService
    {
        public async Task<ServiceResponse<Guid>> Pong(Guid guid)
        {
            return new ServiceResponse<Guid>
            {
                Data = guid,
                Message = "pong",
                Success = true,
            };
        }
    }
}
