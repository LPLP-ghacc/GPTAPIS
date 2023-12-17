using GPTAPIS.Endpoints.TextChat.Assistants;
using GPTAPIS.MessageConstruct.Text;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GPTAPIS.Endpoints.TextChat;

public sealed class AssistantEndPoint : BaseEndpoint
{
    public AssistantEndPoint(HttpClient client, APIService service, bool enableDebug = true)
    {
        Address = "https://api.openai.com/v1/assistants";
        Client = client;
        Service = service;
        EnableDebug = enableDebug;

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Service.ApiKey);
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        Client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
    }

    public override string Address { get; }
    public override HttpClient Client { get; }
    public override APIService Service { get; }
    public override bool EnableDebug { get; }

    /// <summary>
    /// Creates a new assistant. The method returns a full overloaded class of Assistant
    /// </summary>
    /// <param name="assistant"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<Assistant> SendNewAssistant(Assistant assistant)
    {
        if (assistant is null)
        {
            throw new ArgumentNullException(nameof(assistant));
        }

        var payload = new
        {
            model = assistant.Model,
            name = assistant.Name,
            description = assistant.Description,
            instructions = assistant.Instructions,
            tools = assistant.Tools,
            file_ids = assistant.FileIds,
            metadata = assistant.Metadata
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

        var assist = JsonSerializer.Deserialize<Assistant>(responseString);

        if(assist == null)
            Console.WriteLine("Error when JsonSerializer.Deserialize<Assistant>(responseString)");

        return assist;
    }
}
