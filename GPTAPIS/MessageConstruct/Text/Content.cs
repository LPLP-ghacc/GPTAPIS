﻿using System.Text.Json.Serialization;

namespace GPTAPIS.MessageConstruct;

public sealed class Content
{
    public Content() { }

    public Content(ContentType type, string input)
    {
        Type = type;

        switch (Type)
        {
            case ContentType.text:
                Text = input;
                break;
            case ContentType.image_url:
                ImageUrl = new ImageUrl(input);
                break;
        }
    }

    public Content(ImageUrl imageUrl)
    {
        Type = ContentType.image_url;
        ImageUrl = imageUrl;
    }

    [JsonInclude]
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter<ContentType>))]
    public ContentType Type { get; private set; }

    [JsonInclude]
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Text { get; private set; }

    [JsonInclude]
    [JsonPropertyName("image_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ImageUrl ImageUrl { get; private set; }

    public static implicit operator Content(ImageUrl imageUrl) => new Content(imageUrl);
}
