using SpudPI.Shared;
using System.Net;
using System.Text.Json;

namespace SpudPI.API
{
    public class VoicemodConnectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IVoicemodService _voicemodService;

        public VoicemodConnectionMiddleware(RequestDelegate next, IVoicemodService voicemodService)
        {
            _next = next;
            _voicemodService = voicemodService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            //{
            //    context.Response.Clear();

            //    var customMessage = new { message = "Unauthorized Access, reported to MI5" };
            //    var response = JsonSerializer.Serialize(customMessage);

            //    context.Response.ContentType = "application/json";
            //    await context.Response.WriteAsync(response);
            //    return;
            //}


            if (!_voicemodService.IsConnected)
            {
                var serviceResponse = new ServiceResponse<object>
                {
                    Success = false,
                    Data = null,
                    Message = "SpudPI is currently unavailable."
                };

                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                context.Response.ContentType = "application/json";

                var responseJson = JsonSerializer.Serialize(serviceResponse);
                await context.Response.WriteAsync(responseJson);
            }
            else
            {

                await _next(context);
            }
        }
    }
}
