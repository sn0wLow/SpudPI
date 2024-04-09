using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VoicemodAPI.Model;

namespace VoicemodAPI
{
    public class VoiceModAPI
    {
        public event EventHandler? OnDisconnect;

        private ClientWebSocket? webSocket = null;
        private ConcurrentDictionary<string, TaskCompletionSource<string>> responseHandlers = new();
        private readonly SemaphoreSlim wsSemaphore = new SemaphoreSlim(1, 1);

        public async Task ConnectAsync(string apiKey)
        {
            webSocket = new ClientWebSocket();
            Uri serverUri = new Uri("ws://localhost:59129/v1");
            // Ports: 59129, 20000, 39273, 42152, 43782, 46667, 35679, 37170, 38501, 33952, 30546

            await webSocket.ConnectAsync(serverUri, CancellationToken.None);


            var registerClientRequest = new RegisterClientRequest()
            {
                ActionID = Guid.NewGuid().ToString(),
                Payload = new() { ClientKey = apiKey }
            };

            var registerClientResponse = await GetResponse<RegisterClientRequest, RegisterClientResponse>(registerClientRequest);

            var status = registerClientResponse?.Payload?.Status;

            if (status is null)
            {
                var message = "Connecting to Voicemod API failed, response Status was null";
                await DisconnectAsync();
                throw new InvalidOperationException(message);
            }

            if (status.Code is null)
            {
                var message = "Connecting to Voicemod API failed, response Code was null";
                await DisconnectAsync();
                throw new InvalidOperationException(message);
            }

            if (status.Code != "200")
            {
                var code = status.Code;
                var description = status.Description;
                var message = $"Connecting to Voicemod API failed. Code: '{code}', {description} (Wrong API Key?)";

                await DisconnectAsync();
                throw new InvalidOperationException($"{message}");
            }
        }

        public async Task DisconnectAsync()
        {
            responseHandlers.Clear();

            if (webSocket is not null && webSocket.State != WebSocketState.Closed)
            {
                try
                {
                    using var timeoutCts = new CancellationTokenSource(TimeOutThreshold);
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnecting", timeoutCts.Token);

                }
                catch (Exception)
                {

                }
                finally
                {
                    webSocket.Dispose();
                    OnDisconnect?.Invoke(null, new EventArgs());
                }
            }
        }

        public async Task SendRequest<TRequest>(TRequest request) where TRequest : IVoiceModRequest
        {
            if (webSocket == null)
            {
                throw new NullReferenceException("WebSocket is not initialized");
            }

            if (!IsConnected)
            {
                throw new InvalidOperationException("Not connected to the WebSocket");
            }

            using var timeoutCts = new CancellationTokenSource(TimeOutThreshold);
            string message = JsonSerializer.Serialize<TRequest>(request);

            try
            {
                await wsSemaphore.WaitAsync();
                await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, timeoutCts.Token);
            }
            catch (TaskCanceledException)
            {
                responseHandlers.TryRemove(request.ActionID, out _);
                throw new TimeoutException("Sending Request timed out");
            }
            catch (WebSocketException)
            {
                await DisconnectAsync();
                throw;
            }
            finally
            {
                wsSemaphore.Release();
            }
        }

        public async Task PlayMemeSound(Guid id)
        {
            var playMemeSoundRequest = new PlayMemeSoundRequest()
            {
                ActionID = Guid.NewGuid().ToString(),
                Payload = new() { ID = id.ToString(), IsKeyDown = true },
            };

            await SendRequest(playMemeSoundRequest);
        }

        public async Task StopAllMemeSounds()
        {
            var stopAllMemeSoundsRequest = new StopAllMemeSoundsRequest()
            {
                ActionID = Guid.NewGuid().ToString(),
                Payload = new()
            };

            await SendRequest(stopAllMemeSoundsRequest);
        }

        public async Task<TResponse?> GetResponse<TRequest, TResponse>(TRequest request, JsonSerializerOptions? options = null) where TRequest : IVoiceModRequest where TResponse : class, IVoiceModResponse
        {
            if (webSocket is null)
            {
                throw new NullReferenceException("WebSocket is not initialized.");
            }

            if (!IsConnected)
            {
                throw new InvalidOperationException("Not connected to the WebSocket.");
            }

            using var timeoutCts = new CancellationTokenSource(TimeOutThreshold);
            string requestJSON = JsonSerializer.Serialize(request, options);

            try
            {
                await wsSemaphore.WaitAsync();
                await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(requestJSON)), WebSocketMessageType.Text, true, timeoutCts.Token);
            }
            catch (TaskCanceledException)
            {
                responseHandlers.TryRemove(request.ActionID, out _);
                throw new TimeoutException("Sending Request timed out");
            }
            catch (WebSocketException)
            {
                await DisconnectAsync();
                throw;
            }
            finally
            {
                wsSemaphore.Release();
            }

            var tcs = new TaskCompletionSource<string>();
            responseHandlers[request.ActionID] = tcs;


            TResponse? responseObject = await ReceiveWebSocketResponse<TResponse>(request.ActionID, request.Action);
            return responseObject;
        }

        public async Task<GetUserResponse?> GetUserResponse()
        {
            var getUserRequest = new GetUserRequest()
            {
                ActionID = Guid.NewGuid().ToString(),
                Payload = new()
            };

            return await GetResponse<GetUserRequest, GetUserResponse>(getUserRequest);
        }

        public async Task<GetAllMemeSoundsResponse?> GetAllMemeSoundsResponse()
        {
            var getAllMemeSoundsRequest = new GetAllMemeSoundsRequest()
            {
                ActionID = Guid.NewGuid().ToString(),
                Payload = new()
            };

            return await GetResponse<GetAllMemeSoundsRequest, GetAllMemeSoundsResponse>(getAllMemeSoundsRequest);
        }

        public async Task<GetBitmapResponse?> GetBitmapResponse(Guid memeID)
        {
            var getBitmapResponse = new GetBitmapRequest()
            {
                ActionID = Guid.NewGuid().ToString(),
                Payload = new() { MemeID = memeID }
            };

            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var response = await GetResponse<GetBitmapRequest, GetBitmapResponse>(getBitmapResponse, jsonSerializerOptions);

            if (response?.ActionObject?.MemeSoundBitmap is not null)
            {
                response.ActionObject.MemeSoundBitmap.MemeID = memeID;
            }

            return response;
        }

        public async Task<List<Soundboard>> GetAllSoundboards()
        {
            List<Soundboard> soundboards = new List<Soundboard>();
            var getAllSoundboardsResponse = await GetAllSoundboardsResponse();

            if (getAllSoundboardsResponse is null ||
                getAllSoundboardsResponse.ActionObject is null ||
                getAllSoundboardsResponse.ActionObject.Soundboards is null)
            {
                return soundboards;
            }

            foreach (var responseSoundboard in getAllSoundboardsResponse.ActionObject.Soundboards)
            {
                if (responseSoundboard.Sounds is not null)
                {
                    soundboards.Add(responseSoundboard);

                    foreach (var sound in responseSoundboard.Sounds)
                    {
                        sound.IsCustom = responseSoundboard.IsCustom;
                        sound.ShowProLogo = responseSoundboard.ShowProLogo;
                    }
                }
            }



            return soundboards;
        }

        public async Task<GetAllSoundboardsResponse?> GetAllSoundboardsResponse()
        {
            var getAllSoundboardsRequest = new GetAllSoundboardsRequest()
            {
                ActionID = Guid.NewGuid().ToString(),
                Payload = new()
            };

            return await GetResponse<GetAllSoundboardsRequest, GetAllSoundboardsResponse>(getAllSoundboardsRequest);
        }

        public async Task<List<MemeSoundBitmap>> GetAllMemeSoundBitmaps()
        {
            var getAllSoundboardsResponse = await GetAllSoundboardsResponse();
            List<MemeSoundBitmap> bitmaps = new();

            if (getAllSoundboardsResponse is null ||
                getAllSoundboardsResponse.ActionObject is null ||
                getAllSoundboardsResponse.ActionObject.Soundboards is null)
            {
                return bitmaps;
            }

            foreach (var soundboards in getAllSoundboardsResponse.ActionObject.Soundboards)
            {
                if (soundboards.Sounds is null)
                {
                    continue;
                }

                foreach (var sound in soundboards.Sounds)
                {
                    var bitmap = await GetBitmapResponse(sound.ID);

                    if (bitmap is not null &&
                        bitmap.ActionObject is not null &&
                        bitmap.ActionObject.MemeSoundBitmap is not null)
                    {
                        bitmaps.Add(bitmap.ActionObject.MemeSoundBitmap);
                    }
                }
            }

            return bitmaps;
        }

        public async Task<GetAllMemeSoundsResponse?> GetAllMemeSoundsIncludingImagesResponse()
        {
            var getAllMemeSoundsRequest = await GetAllMemeSoundsResponse();

            if (getAllMemeSoundsRequest is not null &&
                getAllMemeSoundsRequest.ActionObject is not null &&
                getAllMemeSoundsRequest.ActionObject.ListOfMemes is not null)
            {
                foreach (var memeSound in getAllMemeSoundsRequest.ActionObject.ListOfMemes)
                {
                    var bitmap = await GetBitmapResponse(memeSound.ID);
                    memeSound.Image = bitmap?.ActionObject?.MemeSoundBitmap?.ImageBase64;
                }
            }

            return getAllMemeSoundsRequest;
        }

        public async Task<GetAllSoundboardsResponse?> GetAllSoundboardsIncludingImagesResponse()
        {
            var getAllSoundboardsRequest = new GetAllSoundboardsRequest()
            {
                ActionID = Guid.NewGuid().ToString(),
                Payload = new()
            };

            var soundboardResponse = await GetResponse<GetAllSoundboardsRequest, GetAllSoundboardsResponse>(getAllSoundboardsRequest);

            if (soundboardResponse is not null
                && soundboardResponse.ActionObject is not null
                && soundboardResponse.ActionObject.Soundboards is not null)
            {
                foreach (var soundboard in soundboardResponse.ActionObject.Soundboards)
                {
                    if (soundboard.Sounds is not null)
                    {
                        foreach (var sound in soundboard.Sounds)
                        {
                            var bitmap = await GetBitmapResponse(sound.ID);
                            if (bitmap is not null &&
                                bitmap.ActionObject is not null &&
                                bitmap.ActionObject.MemeSoundBitmap is not null)
                            {
                                sound.Image = bitmap.ActionObject.MemeSoundBitmap.ImageBase64;
                            }
                        }
                    }
                }
            }

            return soundboardResponse;
        }

        public async Task<GetMuteMemeForMeStatusResponse?> GetMuteMemeForMeStatusResponse()
        {
            var getMuteMemeForMeStatusRequest = new GetMuteMemeForMeStatusRequest()
            {
                ActionID = Guid.NewGuid().ToString(),
                Payload = new()
            };

            return await GetResponse<GetMuteMemeForMeStatusRequest, GetMuteMemeForMeStatusResponse>(getMuteMemeForMeStatusRequest);
        }

        public async Task<GetMuteMemeForMeStatusResponse?> GetToggleMuteMemeForMeResponse()
        {
            var getToggleMuteMemeForMeRequest = new ToggleMuteMemeForMeRequest()
            {
                ActionID = Guid.NewGuid().ToString(),
                Payload = new()
            };


            var getToggleMuteMemeForMeResponse =
                await GetResponse<ToggleMuteMemeForMeRequest, GetMuteMemeForMeStatusResponse>(getToggleMuteMemeForMeRequest);

            // value missing in getToggleMuteMemeForMeResponse
            //var getMuteMemeForMeStatusResponse = await GetMuteMemeForMeStatusResponse();

            var getMuteMemeForMeStatusResponse = await GetMuteMemeForMeStatusResponse();

            if (getMuteMemeForMeStatusResponse is not null &&
                getMuteMemeForMeStatusResponse.ActionObject is not null)
            {
                getToggleMuteMemeForMeResponse ??= new();
                getToggleMuteMemeForMeResponse.ActionObject ??= new();

                getToggleMuteMemeForMeResponse.ActionObject.Value = getMuteMemeForMeStatusResponse.ActionObject.Value;
            }




            return getToggleMuteMemeForMeResponse;
        }

        public async Task HeartBeatTestMessage()
        {
            using var timeoutCts = new CancellationTokenSource(TimeOutThreshold);
            try
            {
                await webSocket!.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("")), WebSocketMessageType.Text, true, timeoutCts.Token);
            }
            catch (Exception)
            {
                await DisconnectAsync();
            }
        }

        private async Task<string> ReceiveMessageAsync(CancellationToken cancellationToken)
        {
            var buffer = new byte[1024 * 4];
            var stringBuilder = new StringBuilder();

            while (!cancellationToken.IsCancellationRequested)
            {
                var result = await webSocket!.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    stringBuilder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                    if (result.EndOfMessage)
                    {
                        return stringBuilder.ToString();
                    }
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    throw new WebSocketException("WebSocket closed message received");
                }
            }

            return string.Empty;
        }

        private async Task<TResponse?> ReceiveWebSocketResponse<TResponse>(string actionID, string? action = null) where TResponse : class, IVoiceModResponse
        {
            using var timeoutCts = new CancellationTokenSource(TimeOutThreshold);
            TResponse? responseObject;


            try
            {
                while (!timeoutCts.Token.IsCancellationRequested)
                {
                    await wsSemaphore.WaitAsync();
                    string response = await ReceiveMessageAsync(timeoutCts.Token);
                    wsSemaphore.Release();

                    try
                    {
                        responseObject = JsonSerializer.Deserialize<TResponse>(response);

                        if (responseObject is not null && responseObject.ActionID is not null)
                        {
                            if (IsMatchingResponse(responseObject, action, actionID))
                            {
                                responseHandlers.TryRemove(actionID, out _);
                                return responseObject;
                            }
                            else if (responseHandlers.TryGetValue(actionID, out var otherTcs))
                            {
                                otherTcs.SetResult(response);
                                responseHandlers.TryRemove(actionID, out _);
                                return responseObject;
                            }
                        }
                    }
                    catch (JsonException)
                    {

                    }
                }
            }
            catch (WebSocketException ex)
            {
                wsSemaphore.Release();
                await DisconnectAsync();
                throw new WebSocketException(ex.Message);
            }
            catch (TaskCanceledException)
            {
                wsSemaphore.Release();
                throw new TimeoutException("Getting Response from Voicemod API timed out");
            }

            return null;
        }

        private bool IsMatchingResponse<TResponse>(TResponse response, string? action, string actionID) where TResponse : IVoiceModResponse
        {
            if (response is IVoiceModResponse)
            {
                if (response.ActionID is not null && response.ActionID.Equals(actionID))
                {
                    return true;
                }
            }

            //if (response is GetAllMemeSoundsResponse && (response as GetAllMemeSoundsResponse)!.ActionID == actionID)
            //{
            //    return true;
            //}
            //else if (response is GetAllSoundboardsResponse && (response as GetAllSoundboardsResponse)!.ActionID == actionID)
            //{
            //    return true;
            //}
            //else if (response is RegisterClientResponse && (response as RegisterClientResponse)!.ActionID == actionID)
            //{
            //    return true;
            //}
            //else if (response is GetMuteMemeForMeStatusResponse && (response as GetMuteMemeForMeStatusResponse)!.ActionID == actionID)
            //{
            //    return true;
            //}
            //else if (response is GetToggleMuteMemeForMeResponse && (response as GetToggleMuteMemeForMeResponse)!.ActionID == actionID)
            //{
            //    return true;
            //}
            //else if (response is GetBitmapResponse && (response as GetBitmapResponse)!.ActionID == actionID)
            //{
            //    var bitmapResponse = (response as GetBitmapResponse)!;

            //    if (bitmapResponse.ActionObject is not null && bitmapResponse.ActionObject.MemeSoundBitmap is not null)
            //    {
            //        bitmapResponse.ActionObject.MemeSoundBitmap.MemeID = bitmapResponse.ActionObject.MemeID;
            //    }


            //    return true;
            //}
            //else if (response is GetUserResponse && (response as GetUserResponse)!.ActionID == actionID)
            //{
            //    return true;
            //}

            return false;
        }

        public TimeSpan TimeOutThreshold { get; set; } = TimeSpan.FromSeconds(30);
        public bool IsConnected => webSocket is not null && webSocket.State == WebSocketState.Open;
    }
}
