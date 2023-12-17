using GPTAPIS.MessageConstruct.Text;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GPTAPIS.Net.Api.Text;

public class ChatResponse
{
    public ChatResponse(List<Choice> choices, long created, string id, string model, string @object, UsageInfo usage)
    {
        Choices = choices;
        Created = created;
        Id = id;
        Model = model;
        Object = @object;
        Usage = usage;
    }

    [JsonInclude]
    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; }

    [JsonInclude]
    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonInclude]
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonInclude]
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonInclude]
    [JsonPropertyName("usage")]
    public UsageInfo Usage { get; set; }
}

public class Choice
{
    public Choice(string finishReason, int index, Message message)
    {
        FinishReason = finishReason;
        Index = index;
        Message = message;
    }

    [JsonInclude]
    [JsonPropertyName("delta")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Delta Delta { get; private set; }

    [JsonInclude]
    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; }

    [JsonInclude]
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonInclude]
    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Message Message { get; set; }
}

public class Delta
{
    public Delta(string content, string role)
    {
        Content = content;
        Role = role;
    }

    [JsonInclude]
    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonInclude]
    [JsonPropertyName("role")]
    public string Role { get; set; }
}

public class MessageContent
{
    public MessageContent(string content, string role, string name = "")
    {
        Content = content;
        Role = role;
        Name = name;
    }

    [JsonInclude]
    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonInclude]
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonInclude]
    [JsonPropertyName("name")]
    public string Name { get; set; } 
}

public class UsageInfo
{
    public UsageInfo(int completionTokens, int promptTokens, int totalTokens)
    {
        CompletionTokens = completionTokens;
        PromptTokens = promptTokens;
        TotalTokens = totalTokens;
    }

    [JsonInclude]
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }

    [JsonInclude]
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonInclude]
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}
