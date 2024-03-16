using System.Net.Http.Json;

namespace SpudPI.Shared.Service
{
    public static class ApiService
    {
        public static async Task<ServiceResponse<TResponse>> PostAsync<TRequest, TResponse>(HttpClient http, string uri, TRequest requestContent)
            where TRequest : class where TResponse : class
        {
            HttpResponseMessage result;
            try
            {
                result = await http.PostAsJsonAsync(uri, requestContent);
                var content = await result.Content.ReadFromJsonAsync<ServiceResponse<TResponse>>();

                if (content is null)
                {
                    return new ServiceResponse<TResponse>
                    {
                        Data = null,
                        Success = false,
                        Message = "Unexpected error: API Response was null"
                    };
                }

                return content;

                //if (result.IsSuccessStatusCode)
                //{

                //}
                //else
                //{
                //    string customMessage = result.StatusCode switch
                //    {
                //        System.Net.HttpStatusCode.MethodNotAllowed =>
                //        $"SpudPI Endpoint '{result?.RequestMessage?.RequestUri}' not accessible",
                //        _ => $"Error: {result.StatusCode} - {result.ReasonPhrase}"
                //    };

                //    return new ServiceResponse<TResponse>
                //    {
                //        Data = null,
                //        Success = false,
                //        Message = customMessage
                //    };
                //}
            }
            catch (HttpRequestException ex)
            {
                var fuck = ex;
                return new ServiceResponse<TResponse>
                {
                    Data = null,
                    Success = false,
                    Message = "SpudPI service is not accessible"
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<TResponse>
                {
                    Data = null,
                    Success = false,
                    Message = $"Unexpected error: {e.Message}"
                };
            }
        }

        public static async Task<ServiceResponse<TResponse>> GetAsync<TRequest, TResponse>(HttpClient http, string uri)
            where TRequest : class where TResponse : class
        {
            HttpResponseMessage result;
            try
            {
                result = await http.GetAsync(uri);
                var content = await result.Content.ReadFromJsonAsync<ServiceResponse<TResponse>>();

                if (content is null)
                {
                    return new ServiceResponse<TResponse>
                    {
                        Data = null,
                        Success = false,
                        Message = "Unexpected error: API Response was null"
                    };
                }

                return content;
            }
            catch (HttpRequestException ex)
            {
                var fuck = ex;
                return new ServiceResponse<TResponse>
                {
                    Data = null,
                    Success = false,
                    Message = "SpudPI service is not accessible"
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<TResponse>
                {
                    Data = null,
                    Success = false,
                    Message = $"Unexpected error: {e.Message}"
                };
            }
        }

    }
}
