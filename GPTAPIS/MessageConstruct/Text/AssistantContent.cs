using System.Text.Json.Serialization;

namespace GPTAPIS.MessageConstruct;

public sealed class AssistantContent
{
    public AssistantContent(string type, MessageText text)
    {
        Type = type;
        Text = text;
    }

    [JsonInclude]
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonInclude]
    [JsonPropertyName("text")]
    public MessageText Text { get; set; }
}
