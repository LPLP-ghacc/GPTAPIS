using GPTAPIS.Net.Api.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GPTAPIS.Endpoints.TextChat.Vision
{
    public sealed class VisionEndpoint : BaseEndpoint
    {
        public VisionEndpoint(HttpClient client, APIService service, bool enableDebug = true)
        {
            Address = "https://api.openai.com/v1/chat/completions";
            Client = client;
            Service = service;
            EnableDebug = enableDebug;

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Service.ApiKey);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override string Address { get; }
        public override HttpClient Client { get; }
        public override APIService Service { get; }
        public override bool EnableDebug { get; }


        //public async Task<ChatResponse> GetCompletionAsync()
        //{
        //
        //}
        //
        //private async Task<ChatResponse> SendRequestAsync()
        //{
        //
        //}
    }
}
