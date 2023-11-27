using System.Runtime.Serialization;

namespace GPTAPIS
{
    public enum ImageDetail
    {
        [EnumMember(Value = "auto")]
        Auto,
        [EnumMember(Value = "low")]
        Low,
        [EnumMember(Value = "high")]
        High
    }
}
