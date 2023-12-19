using GPTAPIS.Endpoints.TextChat.Assistants;
using GPTAPIS.MessageConstruct;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThreadResponse = GPTAPIS.Endpoints.TextChat.Assistants.ThreadResponse;

namespace GPTAPIS.Endpoints.TextChat;

public sealed class AssistantEndpoint : BaseEndpoint
{
    public AssistantEndpoint(HttpClient client, APIService service, ThreadResponse[]? threads = null, bool enableDebug = true)
    {
        Address = "https://api.openai.com/v1/assistants";
        Client = client;
        Service = service;
        EnableDebug = enableDebug;

        Threads = threads == null ? Threads = new List<ThreadResponse>() : threads.ToList();        

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Service.ApiKey);
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        Client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
    }

    public override string Address { get; }
    public override HttpClient Client { get; }
    public override APIService Service { get; }
    public override bool EnableDebug { get; }
    public List<ThreadResponse> Threads { get; set; }

    public async Task<ThreadResponse?> AddThread()
    {
        var thread = await CreateThread();

        if(thread != null)
        {
            Threads.Add(thread);
            return thread;
        }
        else
        {
            if(EnableDebug)
                Console.WriteLine("thread = null");
        }

        return null;
    }

    public async Task<ThreadResponse?> DeleteThread(string threadId)
    {
        string requestUri = $"https://api.openai.com/v1/threads/{threadId}";

        try
        {
            HttpResponseMessage response = await Client.DeleteAsync(requestUri);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            var obj = JsonSerializer.Deserialize<ThreadResponse>(responseBody);

            Console.WriteLine($"Thread {threadId} has been successfully deleted.\n" +
                $"HTTP status: {(int)response.StatusCode} {response.ReasonPhrase}");

            if(obj != null)
            {
                return obj;
            }
            else
            {
                if (EnableDebug) Console.WriteLine("obj == null");
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Ошибка запроса: {e.Message}");
        }

        return null;
    }

    public async Task<string> AddMessage(string message, string threadID)
    {
        var requestBody = new
        {
            inputs = message
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        string requestUri = $"https://api.openai.com/v1/assistants/{threadID}/messages";

        try
        {
            HttpResponseMessage response = await Client.PostAsync(requestUri, contentString);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
        catch (HttpRequestException e)
        {
            return $"Error: {e.Message}";
        }
    }

    /// <summary>
    /// Creates a new assistant. The method returns a full overloaded class of Assistant
    /// </summary>
    public async Task<Assistant?> NewAssistant(Assistant assistant)
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

        return assist != null ? assist : null;
    }

    public static Assistant CreateAssistant(Model model, string name, string instructions)
    {
        Assistant assistant = new()
        {
            Model = ModelConvert.GetModel(model),
            Name = name,
            Instructions = instructions,
            Description = "",
            Tools = new List<Tool>(),
            FileIds = new List<string>(),
            Metadata = new Dictionary<string, object>()
        };

        return assistant;
    }

    public async Task<List<Assistant>> GetAssistants()
    {
        try
        {
            string requestUri = "https://api.openai.com/v1/assistants";

            HttpResponseMessage response = await Client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<AssistantsResponce>(responseBody);

            if (data != null)
            {
                List<Assistant> assistants = new List<Assistant>();

                foreach (var obj in data.Data)
                {
                    var assit = JsonSerializer.Deserialize(obj, typeof(Assistant));
                    assistants.Add(assit);
                }
                return assistants;
            }
            else
            {
                Console.WriteLine("data.Assistants = null");
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

        return new List<Assistant>();
    }

    private async Task<ThreadResponse?> CreateThread()
    {
        var contentString = new StringContent("", Encoding.UTF8, "application/json");
        string requestUri = $"https://api.openai.com/v1/threads";

        ThreadResponse? threadResponce = null;
        try
        {
            HttpResponseMessage response = await Client.PostAsync(requestUri, contentString);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            threadResponce = JsonSerializer.Deserialize<ThreadResponse>(responseBody);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

        return threadResponce == null ? threadResponce : null;
    }
}