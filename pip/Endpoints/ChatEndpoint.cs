using GPTAPIS.Exten;
using GPTAPIS.Net.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace GPTAPIS.Endpoints
{
    public sealed class ChatEndpoint : BaseEndpoint
    {
        public ChatEndpoint(string address, HttpClient client, APIService service, bool enableDebug = true)
        {
            Address = address;
            Client = client;
            Service = service;
            EnableDebug = enableDebug;

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Service.ApiKey);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected override string Address { get; set; }
        protected override HttpClient Client { get; set; }
        protected override APIService Service { get; set; }
        protected override bool EnableDebug { get; }

        public override async Task<ChatResponse> SendRequestAsync(RequestForm request)
        {
            string stringmodel = request.Model;

            var payload = new
            {
                model = stringmodel,
                messages = request.Messages,
                frequency_penalty = request.PresencePenalty,
                logit_bias = request.LogitBias,
                max_tokens = request.MaxTokens,
                presence_penalty = request.PresencePenalty,
                seed = request.Seed,
                stop = request.Stops,
                stream = false,
                temperature = request.Temperature,
                top_p = request.TopP,
                user = request.User,

            };

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var json = JsonSerializer.Serialize(payload, options);

            if(EnableDebug)
                Console.WriteLine(json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(Address, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();

                if (EnableDebug)
                    Console.WriteLine(responseString);

                return JsonSerializer.Deserialize<ChatResponse>(responseString);
            }
            else throw new HttpRequestException($"Error: {response.StatusCode}");
        }

        public override async Task<ChatResponse> SendStreamRequestAsync(RequestForm request, Action<ChatResponse> responseHandler)
        {
            string stringmodel = request.Model;

            var payload = new
            {
                model = stringmodel,
                messages = request.Messages,
                frequency_penalty = request.PresencePenalty,
                logit_bias = request.LogitBias,
                max_tokens = request.MaxTokens,
                presence_penalty = request.PresencePenalty,
                seed = request.Seed,
                stop = request.Stops,
                stream = true,
                temperature = request.Temperature,
                top_p = request.TopP,
                user = request.User,

            };

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var json = JsonSerializer.Serialize(payload, options);

            if (EnableDebug)
                Console.WriteLine(json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PostAsync(Address, content);

            ChatResponse result = null;

            if (response.IsSuccessStatusCode)
            {
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    StreamReader reader = new StreamReader(stream);

                    try
                    {
                        while (true)
                        {
                            string text = await reader.ReadLineAsync().ConfigureAwait(continueOnCapturedContext: false);

                            if (text == null) break;

                            if (text.TryGetEventStreamData(out var eventData) && !string.IsNullOrWhiteSpace(eventData))
                            {
                                ChatResponse delta = JsonSerializer.Deserialize<ChatResponse>(eventData);

                                responseHandler.Invoke(delta);

                                result = delta;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                    }
                    finally
                    {
                        if (reader != null) reader.Dispose(); stream.Dispose();
                    }
                }
                return result;
            }
            else throw new HttpRequestException($"Error: {response.StatusCode}");
        }

        public override Task<ChatResponse> SendRequest(RequestForm request)
        {
            string stringmodel = request.Model;

            var payload = new
            {
                model = stringmodel,
                messages = request.Messages
            };

            var json = JsonSerializer.Serialize(payload);

            if (EnableDebug)
                Console.WriteLine(json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = Client.PostAsync(Address, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(responseString);

                return null;
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }
        }
    }
}
