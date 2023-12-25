using GPTAPIS.Endpoints.TextChat.Assistants;
using GPTAPIS.MessageConstruct;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using ThreadResponse = GPTAPIS.Endpoints.TextChat.Assistants.ThreadResponse;

namespace GPTAPIS.Endpoints.TextChat;

public sealed class AssistantEndpoint : BaseEndpoint
{
    public Action? OnStatusQueued;
    public Action? OnStatusInProgress;
    public Action? OnStatusCompleted;

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
    public ThreadResponse lastThread = new ThreadResponse();

    public async Task<ThreadResponse?> AddThread()
    {
        var thread = await CreateThread();

        if(thread != null)
        {
            Threads.Add(thread);
            lastThread = thread;
            return thread;
        }
        else
        {
            if(EnableDebug)
                Console.WriteLine("thread = null");
        }

        return null;
    }

    public async Task<string?> AddMessage(Message message, string threadID)
    {
        if (string.IsNullOrEmpty(threadID) && EnableDebug) 
            Console.WriteLine("threadID = null");

        var requestBody = new
        {
            role = message.Role,
            content = message.Content
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        if (EnableDebug) Console.WriteLine(jsonContent);
        var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        string requestUri = $"https://api.openai.com/v1/threads/{threadID}/messages";
        var result = await SendRequest(requestUri, contentString);

        return result != null ? result : null;
    }

    public async Task<ThreadResponse?> RunThread(string threadID, string assistantID)
    {
        if (string.IsNullOrEmpty(assistantID) && EnableDebug)
            Console.WriteLine($"assistantID = null");

        var requestBody = new
        {
            assistant_id = assistantID
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        if (EnableDebug) Console.WriteLine(jsonContent);

        var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        string requestUri = $"https://api.openai.com/v1/threads/{threadID}/runs";
        var response = await SendRequest(requestUri, contentString);
        if(response != null)
        {
            ThreadResponse? result = JsonSerializer.Deserialize<ThreadResponse>(response);
            return result;
        }

        return null;
    }

    public async Task<ThreadResponse?> GetRunStatus(string threadID, string runID)
    {
        string requestUri = $"https://api.openai.com/v1/threads/{threadID}/runs/{runID}";
        HttpResponseMessage response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        ThreadResponse? result = JsonSerializer.Deserialize<ThreadResponse>(responseBody);
        return result != null ? result : null;
    }

    /// <summary>
    /// Method returns a statusCode relative to the request, where 0 is queued, 1 is in_progress, and 2 is completed.<br/> 
    /// When the statusCode reaches 2, the method exits the loop body.
    /// </summary>
    public async Task<int> FromStatusCode(string threadID, string runID)
    {
        int statusCode = 0;

        while (true)
        {
            var response = await GetRunStatus(threadID, runID);

            if (response == null)
            {
                throw new Exception("Response from GetRunStatus = null");
            }

            switch (response.Status)
            {
                case ("queued"):
                    statusCode = 0;

                    if (EnableDebug) Console.WriteLine($"StatusCode : {response.Status} ({statusCode})");
                    OnStatusQueued?.Invoke();
                    break;
                case ("in_progress"):
                    statusCode = 1;

                    if (EnableDebug) Console.WriteLine($"StatusCode : {response.Status} ({statusCode})");
                    OnStatusInProgress?.Invoke();
                    break;
                case ("completed"):
                    statusCode = 2;

                    if (EnableDebug) Console.WriteLine($"StatusCode : {response.Status} ({statusCode})");
                    OnStatusCompleted?.Invoke();
                    break;
                default:
                    throw new Exception("Unknown status");
            }

            if (statusCode == 2)
            {
                break;
            }

            await Task.Delay(1000);
        }

        return statusCode;
    }

    public async Task<ThreadResponse[]?> GetMessages(string threadID)
    {
        string requestUri = $"https://api.openai.com/v1/threads/{threadID}/messages";

        HttpResponseMessage response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        ThreadResponse? response1 = JsonSerializer.Deserialize<ThreadResponse?>(responseBody);        
        
        if(response1 != null)
        {
            return response1.Data;          
        }
        else
        {
            return null;
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

        string? response = await SendRequest(requestUri, contentString);

        if(response != null)
        {
            ThreadResponse? result = JsonSerializer.Deserialize<ThreadResponse>(response);

            return result;
        }

        return null;
    }

    public async Task DeleteLastThread()
    {
        await DeleteThread(lastThread.Id);
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

            if (obj != null)
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

    private async Task<string?> SendRequest(string requestUri, StringContent contentString)
    {
        if (EnableDebug) Console.WriteLine($"requestUri : {requestUri}");
        
        try
        {
            HttpResponseMessage response = await Client.PostAsync(requestUri, contentString);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error: {e.Message}");
            return null;
        }
    }
}