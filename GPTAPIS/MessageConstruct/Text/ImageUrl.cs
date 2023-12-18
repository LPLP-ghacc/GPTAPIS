using System.Text.Json.Serialization;

namespace GPTAPIS.MessageConstruct;

public sealed class ImageUrl
{
    [JsonConstructor]
    public ImageUrl(string url, ImageDetail detail = ImageDetail.Auto)
    {
        Url = url;
        Detail = detail;
    }

    [JsonInclude]
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Url { get; private set; }

    [JsonInclude]
    [JsonPropertyName("detail")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ImageDetail Detail { get; private set; }
}
