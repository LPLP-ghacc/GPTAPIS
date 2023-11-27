using GPTAPIS.Extensions;
using GPTAPIS.MessageConstruct.Enums;
using GPTAPIS.Net.Api.Text;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace GPTAPIS.Endpoints;

public sealed class ChatEndpoint : BaseEndpoint
{
    public ChatEndpoint(HttpClient client, APIService service, bool enableDebug = true)
    {
        Address = "https://api.openai.com/v1/chat/completions";
        Client = client;
        Service = service;
        EnableDebug = enableDebug;

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Service.ApiKey);
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public override string Address { get; }
    public override HttpClient Client { get; }
    public override APIService Service { get; }
    public override bool EnableDebug { get; }

    public async Task<ChatResponse> SendRequestAsync(ChatRequestForm request)
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

        if (EnableDebug)
            Console.WriteLine(json);

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await Client.PostAsync(Address, content);

        var responseString = await response.Content.ReadAsStringAsync();

        if (EnableDebug)
            Console.WriteLine(responseString);

        return JsonSerializer.Deserialize<ChatResponse>(responseString);
    }

    public async Task<ChatResponse> SendStreamRequestAsync(ChatRequestForm request, Action<ChatResponse> responseHandler, CancellationToken cancellationToken = default)
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
        using var req = new HttpRequestMessage(HttpMethod.Post, Address);
        req.Content = content;

        HttpResponseMessage response = await Client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        ChatResponse result = null;

        await using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        using var reader = new StreamReader(stream);

        string complitedResponseContent = string.Empty;

        while (await reader.ReadLineAsync().ConfigureAwait(false) is { } streamData)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!streamData.TryGetEventStreamData(out var eventData))
                continue;

            if (string.IsNullOrWhiteSpace(eventData))
                continue;

            var partialResponse = JsonSerializer.Deserialize<ChatResponse>(eventData);
            result = partialResponse;
            complitedResponseContent += partialResponse.Choices[0].Delta.Content;
            responseHandler?.Invoke(partialResponse);
        }
        result.Choices[0].Message = new MessageContent(complitedResponseContent, Role.assistant.ToString());

        return result;
    }

    public Task<ChatResponse> SendRequest(ChatRequestForm request)
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
