using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using GPTAPIS.MessageConstruct.Enums;

namespace GPTAPIS.MessageConstruct.Text
{
    public class Message
    {
        public Message(Role role, IEnumerable<Content> content)
        {
            Role = role.ToString();
            Content = content.ToList();
        }

        public Message(Role role, string content)
        {
            Role = role.ToString();
            Content = content;
        }

        [JsonInclude]
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonInclude]
        [JsonPropertyName("content")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public dynamic Content { get; set; }
    }
}
