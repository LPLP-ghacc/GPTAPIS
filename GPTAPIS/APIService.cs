using GPTAPIS.Endpoints;
using GPTAPIS.MessageConstruct.Text;
using GPTAPIS.Net.Api.Text;

namespace GPTAPIS;

public class APIService
{
    public APIService(string apiKey, HttpClient client)
    {
        ApiKey = apiKey;

        HttpClient = client;

        chatEndpoint = new ChatEndpoint(client, this);
    }

    public string ApiKey { get; set; }
    public HttpClient HttpClient { get; set; }

    public ChatEndpoint chatEndpoint { get; private set; }

    #region Text generation
    public async Task<ChatResponse> GetCompletionAsync(List<Message> messages, Model model)
    {
        var request = new ChatRequestForm(messages, model, null, null, null, null, null, null, null, false);
        var response = await chatEndpoint.SendRequestAsync(request);
        return response;
    }

    /// <summary>
    /// When using GetStreamCompletionAsync, access delta via action. 
    /// Receiving the final Message through the result of Task<ChatResponse>
    /// </summary>
    /// <returns></returns>
    public async Task<ChatResponse> GetStreamCompletionAsync(List<Message> messages, Model model, Action<ChatResponse> deltaHandler)
    {
        var request = new ChatRequestForm(messages, model, null, null, null, null, null, null, null, true);
        var response = await chatEndpoint.SendStreamRequestAsync(request, deltaHandler);
        return response;
    }

    /// <summary>
    /// When using GetStreamCompletionAsync, access delta via action. 
    /// Receiving the final Message through the result of Task<ChatResponse>.
    /// To get the logic of data entry by fields see RequestForm
    /// </summary>
    /// <returns></returns>
    public async Task<ChatResponse> GetStreamCompletionAsync(
        IReadOnlyList<Message> messages,
        Action<ChatResponse> deltaHandler,
        Model model,
        double? frequencyPenalty = 0,
        IReadOnlyDictionary<string, double> logitBias = null,
        int? maxTokens = 4096,
        int? number = 1,
        double? presencePenalty = 0,
        int? seed = 0,
        string[] stops = null,
        double? temperature = 1,
        double? topP = 1,
        dynamic toolChoice = null,
        string user = null)
    {
        var request = new ChatRequestForm(messages, model, frequencyPenalty, logitBias, maxTokens, number, presencePenalty, seed, stops, true, temperature, topP, toolChoice, user);
        var response = await chatEndpoint.SendStreamRequestAsync(request, deltaHandler);
        return response;
    }
    #endregion
}
