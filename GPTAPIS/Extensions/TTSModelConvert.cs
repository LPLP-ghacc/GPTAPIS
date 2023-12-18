using GPTAPIS.MessageConstruct;

namespace GPTAPIS.Net.Api.Speech
{
    public static class TTSModelConvert
    {
        public static string GetStringTTSModel(this TTSModel model)
        {
            switch (model)
            {
                case TTSModel.tts_1:
                    return "tts-1";
                case TTSModel.tts_1_hd:
                    return "tts-1-hd";
                default:
                    return "tts-1";
            }
        }
    }
}
