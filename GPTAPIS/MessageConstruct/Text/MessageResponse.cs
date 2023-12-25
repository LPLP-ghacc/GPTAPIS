using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GPTAPIS.MessageConstruct.Text;

public class MessageResponse
{
    public MessageResponse(string id, string @object, long createdAt, string threadId, string role, List<Content> content, List<object> fileIds, string assistantId, string runId, Dictionary<string, object>? metadata)
    {
        Id = id;
        Object = @object;
        CreatedAt = createdAt;
        ThreadId = threadId;
        Role = role;
        Content = content;
        FileIds = fileIds;
        AssistantId = assistantId;
        RunId = runId;
        Metadata = metadata;
    }

    [JsonInclude]
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Id { get; set; }

    [JsonInclude]
    [JsonPropertyName("object")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Object { get; set; }

    [JsonInclude]
    [JsonPropertyName("created_at")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long CreatedAt { get; set; }

    [JsonInclude]
    [JsonPropertyName("thread_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string ThreadId { get; set; }

    [JsonInclude]
    [JsonPropertyName("role")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Role { get; set; }

    [JsonInclude]
    [JsonPropertyName("content")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<Content> Content { get; set; }

    [JsonInclude]
    [JsonPropertyName("file_ids")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<object> FileIds { get; set; }

    [JsonInclude]
    [JsonPropertyName("assistant_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string AssistantId { get; set; }

    [JsonInclude]
    [JsonPropertyName("run_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string RunId { get; set; }

    [JsonInclude]
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Dictionary<string, object>? Metadata { get; set; }
}
