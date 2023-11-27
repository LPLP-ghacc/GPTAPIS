using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTAPIS.Endpoints.Audio
{
    public sealed class SpeechEndpoint : BaseEndpoint
    {
        public SpeechEndpoint(HttpClient client, APIService service, bool enableDebug)
        {
            Address = "https://api.openai.com/v1/audio/speech";
            Client = client;
            Service = service;
            EnableDebug = enableDebug;
        }

        public override string Address { get; }
        public override HttpClient Client { get; }
        public override APIService Service { get; }
        public override bool EnableDebug { get; }

        //ох заеб....

        //public async Task<string> GetAudioByText()
        //{
        //
        //}
    }
}
