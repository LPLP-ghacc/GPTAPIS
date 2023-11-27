using System;
using System.Net.Http;
using System.Text;

namespace GPTAPIS.Exten
{
    internal static class StringExtensions
    {
        public static bool TryGetEventStreamData(this string streamData, out string eventData)
        {
            eventData = string.Empty;
            if (streamData.StartsWith("data: "))
            {
                int length = "data: ".Length;
                eventData = streamData.Substring(length, streamData.Length - length).Trim();
            }

            return eventData != "[DONE]";
        }

        public static StringContent ToJsonStringContent(this string json, bool debug)
        {
            if (debug)
            {
                Console.WriteLine(json);
            }

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
