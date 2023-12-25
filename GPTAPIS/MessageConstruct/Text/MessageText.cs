using System.Text.Json.Serialization;

namespace GPTAPIS.MessageConstruct;

public class MessageText
{
    public MessageText(string value, List<string> annotations)
    {
        Value = value;
        Annotations = annotations;
    }

    [JsonInclude]
    [JsonPropertyName("value")]
    public string Value { get; private set; }

    [JsonInclude]
    [JsonPropertyName("annotations")]
    public List<string> Annotations { get; private set; }
}
