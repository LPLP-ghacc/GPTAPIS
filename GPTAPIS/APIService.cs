using GPTAPIS.Endpoints.TextChat;

namespace GPTAPIS;

public class APIService
{
    public APIService(string apiKey, HttpClient client, string username = "")
    {
        ApiKey = apiKey;
        Username = username;
        HttpClient = client;
        ChatEndpoint = new ChatEndpoint(client, this);
        AssistantEndPoint = new AssistantEndPoint(client, this);
    }

    public string ApiKey { get; set; }
    public string Username { get; set; }
    public HttpClient HttpClient { get; set; }
    public ChatEndpoint ChatEndpoint { get; private set; }
    public AssistantEndPoint AssistantEndPoint { get; private set;}
}
