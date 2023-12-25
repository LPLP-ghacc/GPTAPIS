using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GPTAPIS.MessageConstruct;

public enum ContentType
{
    [JsonInclude]
    [JsonPropertyName("text")]
    [EnumMember(Value = "text")]
    text,

    [JsonInclude]
    [JsonPropertyName("image_url")]
    [EnumMember(Value = "image_url")]
    image_url,
}
