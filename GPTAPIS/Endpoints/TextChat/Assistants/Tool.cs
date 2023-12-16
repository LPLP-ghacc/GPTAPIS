using System.Text.Json.Serialization;

namespace GPTAPIS.Endpoints.TextChat.Assistants;

public class Tool
{
    public Tool(string type)
    {
        Type = type;
    }

    /// <summary>
    /// Code interpreter, Retrieval, Function
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public static class ToolExtension
{
    public static string? GetStringTool(Tools tool)
    {
        return tool switch
        {
            Tools.CodeInterpreter => "Code interpreter",
            Tools.Retrieval => "Retrieval",
            Tools.Function => "?",
            _ => null,
        };
    }

    public enum Tools
    {
        CodeInterpreter,
        Retrieval,
        Function
    }
}


