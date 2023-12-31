﻿using GPTAPIS.MessageConstruct;
using System.Text.Json.Serialization;

namespace GPTAPIS.Net.Api.Text;

public class ChatRequestForm
{
    /// <summary>
    /// Standart Form
    /// </summary>
    public ChatRequestForm(IReadOnlyList<Message> messages, string model, bool stream)
    {
        Messages = messages;
        Model = model;

        FrequencyPenalty = 0;
        LogitBias = null;
        MaxTokens = 4096;
        Number = 1;
        PresencePenalty = 0;
        Seed = 0;
        Stops = null;
        Stream = stream;
        Temperature = 1;
        TopP = 1;
        ToolChoice = null;
        User = "User";
    }

    public ChatRequestForm(IReadOnlyList<Message> messages,
        Model model,
        double? frequencyPenalty = 0,
        IReadOnlyDictionary<string, double> logitBias = null,
        int? maxTokens = 4096,
        int? number = 1,
        double? presencePenalty = 0,
        int? seed = 0,
        string[] stops = null,
        bool stream = true,
        double? temperature = 1,
        double? topP = 1,
        dynamic toolChoice = null,
        string user = null)
    {
        Messages = messages;
        Model = ModelConvert.GetModel(model);
        LogitBias = logitBias ?? new Dictionary<string, double>();
        FrequencyPenalty = frequencyPenalty;
        MaxTokens = maxTokens;
        Number = number;
        PresencePenalty = presencePenalty;
        Seed = seed;
        Stops = stops ?? Array.Empty<string>();
        Stream = stream;
        Temperature = temperature;
        TopP = topP;
        ToolChoice = toolChoice;
        User = user; 
    }

    /// <summary>
    /// The messages to generate chat completions for, in the chat format.
    /// </summary>
    [JsonPropertyName("messages")]
    public IReadOnlyList<Message> Messages { get; set; }

    /// <summary>
    /// ID of the model to use.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }

    /// <summary>
    /// Number between -2.0 and 2.0.
    /// Positive values penalize new tokens based on their existing frequency in the text so far,
    /// decreasing the model's likelihood to repeat the same line verbatim.<br/>
    /// Defaults to 0
    /// </summary>
    [JsonPropertyName("frequency_penalty")]
    public double? FrequencyPenalty { get; set; }

    /// <summary>
    /// Modify the likelihood of specified tokens appearing in the completion.
    /// Accepts a json object that maps tokens(specified by their token ID in the tokenizer)
    /// to an associated bias value from -100 to 100. Mathematically, the bias is added to the logits
    /// generated by the model prior to sampling.The exact effect will vary per model, but values between
    /// -1 and 1 should decrease or increase likelihood of selection; values like -100 or 100 should result
    /// in a ban or exclusive selection of the relevant token.<br/>
    /// Defaults to null
    /// </summary>
    [JsonPropertyName("logit_bias")]
    public IReadOnlyDictionary<string, double> LogitBias { get; set; }

    /// <summary>
    /// The maximum number of tokens allowed for the generated answer.
    /// By default, the number of tokens the model can return will be (4096 - prompt tokens).
    /// </summary>
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    /// <summary>
    /// How many chat completion choices to generate for each input message.<br/>
    /// Defaults to 1
    /// </summary>
    [JsonPropertyName("n")]
    public int? Number { get; set; }

    /// <summary>
    /// Number between -2.0 and 2.0.
    /// Positive values penalize new tokens based on whether they appear in the text so far,
    /// increasing the model's likelihood to talk about new topics.<br/>
    /// Defaults to 0
    /// </summary>
    [JsonPropertyName("presence_penalty")]
    public double? PresencePenalty { get; set; }

    /// <summary>
    /// This feature is in Beta. If specified, our system will make a best effort to sample deterministically,
    /// such that repeated requests with the same seed and parameters should return the same result.
    /// Determinism is not guaranteed, and you should refer to the system_fingerprint response parameter to
    /// monitor changes in the backend.
    /// </summary>
    [JsonPropertyName("seed")]
    public int? Seed { get; set; }

    /// <summary>
    /// Up to 4 sequences where the API will stop generating further tokens.
    /// </summary>
    [JsonPropertyName("stop")]
    public string[] Stops { get; set; }

    /// <summary>
    /// Specifies where the results should stream and be returned at one time.
    /// Do not set this yourself, use the appropriate methods on <see cref="ChatEndpoint"/> instead.<br/>
    /// Defaults to false
    /// </summary>
    [JsonPropertyName("stream")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Stream { get; internal set; }

    /// <summary>
    /// What sampling temperature to use, between 0 and 2.
    /// Higher values like 0.8 will make the output more random, while lower values like 0.2 will
    /// make it more focused and deterministic.
    /// We generally recommend altering this or top_p but not both.<br/>
    /// Defaults to 1
    /// </summary>
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }

    /// <summary>
    /// An alternative to sampling with temperature, called nucleus sampling,
    /// where the model considers the results of the tokens with top_p probability mass.
    /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
    /// We generally recommend altering this or temperature but not both.<br/>
    /// Defaults to 1
    /// </summary>
    [JsonPropertyName("top_p")]
    public double? TopP { get; set; }

    /// <summary>
    /// Controls which (if any) function is called by the model.<br/>
    /// 'none' means the model will not call a function and instead generates a message.&lt;br/&gt;
    /// 'auto' means the model can pick between generating a message or calling a function.&lt;br/&gt;
    /// Specifying a particular function via {"type: "function", "function": {"name": "my_function"}}
    /// forces the model to call that function.<br/>
    /// 'none' is the default when no functions are present.<br/>
    /// 'auto' is the default if functions are present.<br/>
    /// </summary>
    [JsonPropertyName("tool_choice")]
    public dynamic ToolChoice { get; set; }

    /// <summary>
    /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
    /// </summary>
    [JsonPropertyName("user")]
    public string User { get; set; }

    /// <summary>
    /// Pass "auto" to let the OpenAI service decide, "none" if none are to be called,
    /// or "functionName" to force function call. Defaults to "auto".
    /// </summary>
    [Obsolete("Use ToolChoice")]
    [JsonPropertyName("function_call")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public dynamic FunctionCall { get; }
}
