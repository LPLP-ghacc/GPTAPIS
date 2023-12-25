using System.Text.Json.Serialization;

namespace GPTAPIS.Endpoints.TextChat.Assistants;

public class AssistantsResponce
{
    public AssistantsResponce(string @object, List<dynamic> data)
    {
        Object = @object;
        Data = data;
    }

    [JsonInclude]
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonInclude]
    [JsonPropertyName("data")]
    public List<dynamic> Data { get; set; }
}
