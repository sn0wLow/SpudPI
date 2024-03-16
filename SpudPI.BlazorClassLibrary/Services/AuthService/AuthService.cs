using Microsoft.AspNetCore.Components.Authorization;
using SpudPI.Shared;
using SpudPI.Shared.Service;
using System.Net.Http.Json;
namespace SpudPI.BlazorClassLibrary
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient http;
        private readonly AuthenticationStateProvider authStateProvider;

        public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider)
        {
            this.http = http;
            this.authStateProvider = authStateProvider;
        }

        public async Task<ServiceResponse<string>> Login(LoginDTO request)
        {
            return await ApiHelper.PostAsync<LoginDTO, string>(http, "api/auth/login", request);
        }

        public async Task<bool> IsUserAuthenticated()
        {
            return (await this.authStateProvider.GetAuthenticationStateAsync()).User.Identity!.IsAuthenticated;
        }

    }
}
