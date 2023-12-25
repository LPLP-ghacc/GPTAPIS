using GPTAPIS.MessageConstruct;
using System.Text.Json.Serialization;

namespace GPTAPIS.Endpoints.TextChat.Assistants;

public class ThreadResponse
{
    public ThreadResponse()
    {
    }

    public ThreadResponse(string id, string @object, ThreadResponse[] data, long createdAt, string? threadId, string? role, string status, AssistantContent[] content, List<string>? fileIds, string? assistantId, string? runId, Dictionary<string, object>? metadata, bool? deleted)
    {
        Id = id;
        Object = @object;
        Data = data;
        CreatedAt = createdAt;
        ThreadId = threadId;
        Role = role;
        Status = status;
        Content = content;
        FileIds = fileIds;
        AssistantId = assistantId;
        RunId = runId;
        Metadata = metadata;
        Deleted = deleted;
    }

    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonInclude]
    [JsonPropertyName("object")]
    public string Object { get; set; }

    /// <summary>
    /// Thread message array 
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("data")]
    public ThreadResponse[] Data { get; set; }

    [JsonInclude]
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonInclude]
    [JsonPropertyName("thread_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ThreadId { get; set; }

    [JsonInclude]
    [JsonPropertyName("role")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Role { get; set; }

    [JsonInclude]
    [JsonPropertyName("status")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Status { get; set; }

    [JsonInclude]
    [JsonPropertyName("content")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AssistantContent[] Content { get; set; }

    [JsonInclude]
    [JsonPropertyName("file_ids")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string>? FileIds { get; set; }

    [JsonInclude]
    [JsonPropertyName("assistant_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? AssistantId { get; set; }

    [JsonInclude]
    [JsonPropertyName("run_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? RunId { get; set; }

    [JsonInclude]
    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    [JsonInclude]
    [JsonPropertyName("deleted")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? Deleted { get; set; }
}
