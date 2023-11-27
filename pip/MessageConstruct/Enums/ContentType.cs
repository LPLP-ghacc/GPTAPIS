using System.Runtime.Serialization;

namespace GPTAPIS
{
    public enum ContentType
    {
        [EnumMember(Value = "text")]
        Text,
        [EnumMember(Value = "image_url")]
        ImageUrl
    }
}
