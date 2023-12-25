using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GPTAPIS.MessageConstruct;

public enum Role
{
    [JsonInclude]
    [JsonPropertyName("system")]
    [EnumMember(Value = "system")]
    system = 1,
    [JsonInclude]
    [JsonPropertyName("assistant")]
    [EnumMember(Value = "assistant")]
    assistant,
    [JsonInclude]
    [JsonPropertyName("user")]
    [EnumMember(Value = "user")]
    user,
    [JsonInclude]
    [JsonPropertyName("tool")]
    [EnumMember(Value = "tool")]
    tool
}