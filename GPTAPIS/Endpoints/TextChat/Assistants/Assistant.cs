using GPTAPIS.MessageConstruct;
using System.Text.Json.Serialization;

namespace GPTAPIS.Endpoints.TextChat.Assistants;

public class Assistant
{
    public Assistant()
    {
    }

    /// <summary>
    /// For deserialization
    /// </summary>
    public Assistant(string iD,
                     string @object,
                     long createdAt,
                     Model model,
                     string name,
                     string description,
                     string instructions,
                     List<Tool> tools,
                     List<string> fileIds,
                     Dictionary<string, object> metadata)
    {
        ID = iD;
        Object = @object;
        CreatedAt = createdAt;
        Model = ModelConvert.GetModel(model);
        Name = name;
        Description = description;
        Instructions = instructions;
        Tools = tools;
        FileIds = fileIds;
        Metadata = metadata;
    }

    /// <summary>
    /// do not specify when creating a new
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("id")]
    public string ID { get; set; }

    /// <summary>
    /// do not specify when creating a new
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("object")]
    public string Object { get; set; }

    /// <summary>
    /// do not specify when creating a new
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonInclude]
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonInclude]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonInclude]
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonInclude]
    [JsonPropertyName("instructions")]
    public string Instructions { get; set; }

    [JsonInclude]
    [JsonPropertyName("tools")]
    public List<Tool> Tools { get; set; }

    [JsonInclude]
    [JsonPropertyName("file_ids")]
    public List<string> FileIds { get; set; }

    [JsonInclude]
    [JsonPropertyName("metadata")]
    public Dictionary<string, object> Metadata
    {
        get; set;
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
}
