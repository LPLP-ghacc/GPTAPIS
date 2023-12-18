using GPTAPIS.Endpoints.TextChat;
using GPTAPIS.Endpoints.TextChat.Vision;
using GPTAPIS.Net;
using System.IO;
using System.Net;

namespace GPTAPIS;

public class APIService
{
    public APIService(string apiKey, HttpClient client, string username = "")
    {
        ApiKey = apiKey;
        Username = username;
        HttpClient = client;
        ChatEndpoint = new ChatEndpoint(HttpClient, this);
        AssistantEndPoint = new AssistantEndPoint(HttpClient, this);
        VisionEndpoint = new VisionEndpoint(HttpClient, this);
    }

    /// <summary>
    /// When using profiles
    /// </summary>
    public APIService(string profilePath)
    {
        var profile = Profiling.LoadProfile(profilePath);

        ApiKey = profile.Apikey;
        Username = profile.UserName;

        if (profile.ProxyUrl != null && profile.ProxyPort != null)
        {
            if (profile.ProxyUsername != null && profile.ProxyPassword != null)
            {
                HttpClient = ClientCreator.GetProxyClient(profile.ProxyUrl, profile.ProxyPort,
                    new NetworkCredential(profile.ProxyUsername, profile.ProxyPassword));
            }
            else
            {
                HttpClient = ClientCreator.GetProxyClient(profile.ProxyUrl, profile.ProxyPort);
            }
        }
        else
        {
            HttpClient = new HttpClient();
        }

        ChatEndpoint = new ChatEndpoint(HttpClient, this);
        AssistantEndPoint = new AssistantEndPoint(HttpClient, this);
        VisionEndpoint = new VisionEndpoint(HttpClient, this);
    }

    public string ApiKey { get; set; }
    public string Username { get; set; }
    public HttpClient HttpClient { get; set; }
    public ChatEndpoint ChatEndpoint { get; private set; }
    public AssistantEndPoint AssistantEndPoint { get; private set;}
    public VisionEndpoint VisionEndpoint { get; private set; }

    public static async Task SaveProfile(GPTAPISProfile profile, string? path = null)
    {
        string savePath = string.IsNullOrEmpty(path) ? Environment.CurrentDirectory + $"/Profiles/{profile.UserName}.json" : path;

        await Profiling.ProfileSaveAsync(profile, savePath);
    }
}
