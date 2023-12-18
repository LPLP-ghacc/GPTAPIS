using System.Text.Json.Serialization;
using GPTAPIS.MessageConstruct;

namespace GPTAPIS.Net.Api.Speech
{
    public class SpeechRequestForm
    {
        public SpeechRequestForm(TTSModel model, string input, string voice)
        {
            Model = model.GetStringTTSModel();
            Input = input;
            Voice = voice;
            ResponseFormat = "mp3";
            Speed = 1;
        }

        public SpeechRequestForm(TTSModel model, string input, string voice, string responseFormat, int speed)
        {
            Model = model.GetStringTTSModel();
            Input = input;
            Voice = voice;
            ResponseFormat = responseFormat;
            Speed = speed;
        }

        /// <summary>
        /// See TTSModel
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; }

        /// <summary>
        /// The text to generate audio for. The maximum length is 4096 characters.
        /// </summary>
        [JsonPropertyName("input")]
        public string Input { get; set; }

        /// <summary>
        /// The voice to use when generating the audio. Supported voices are alloy, echo, fable, onyx, nova, and shimmer.
        /// </summary>
        [JsonPropertyName("voice")]
        public string Voice { get; set; }

        /// <summary>
        /// The format to audio in. Supported formats are mp3, opus, aac, and flac.
        /// </summary>
        [JsonPropertyName("response_format")]
        public string ResponseFormat { get; set; }

        /// <summary>
        /// The speed of the generated audio. Select a value from 0.25 to 4.0. 1.0 is the default.
        /// </summary>
        [JsonPropertyName("speed")]
        public int Speed { get; set; }
    }
}
